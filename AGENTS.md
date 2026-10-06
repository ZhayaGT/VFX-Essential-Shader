# VFX Essential Shader: Handoff Notes for AI Assistants

This document is written for AI coding assistants (and the people working with them) who are asked to modify VFX Essential Shader. It explains how the shader is organized, why it is built the way it is, which conventions a change must respect, and where mistakes are easy to make. Read it together with the comments in the source files before changing anything.

For what each property does from an artist's point of view, see [README.md](README.md). This document covers the implementation side.

Statements below describe the **current implementation** unless they are marked **Rule**. A rule is a convention that existing behavior depends on; new code should follow it, and changing it means changing every place that relies on it.

## Contents

- [1. What this shader is](#1-what-this-shader-is)
- [2. Files](#2-files)
- [3. How a pixel is built](#3-how-a-pixel-is-built)
- [4. Modules and keywords](#4-modules-and-keywords)
- [5. Conventions](#5-conventions)
- [6. Inspector name to code name](#6-inspector-name-to-code-name)
- [7. The Inspector script](#7-the-inspector-script)
- [8. Common tasks](#8-common-tasks)
- [9. How to verify a change](#9-how-to-verify-a-change)
- [10. Pitfalls](#10-pitfalls)
- [11. Known limitations](#11-known-limitations)

## 1. What this shader is

- A general-purpose, unlit effects shader for Unity's Universal Render Pipeline (URP 17, Unity 6, Forward path).
- Designed for the built-in Particle System (Shuriken). It also works on a Mesh Renderer.
- One transparent pass. The optional modules (Diffuse 2, Mask, Dissolve, Distortion, Vertex Offset, Rim, Depth) are compiled in or out with shader keywords. Diffuse 1, Tint Color, Emission Scaler, Saturation, and Contrast always run; Wrap Mode, speed toggles, and a few options are plain numeric properties, not keywords.
- The Inspector is hand-written (IMGUI) and is part of the product. The shader is meant to be driven by artists, so a property that exists in the shader but is not drawn by the Inspector is effectively invisible.
- The prototype was assembled by the author in a node-based shader editor. This release is a hand-written HLSL rewrite of that prototype. Some property names still reflect the prototype (see [Pitfalls](#10-pitfalls)).

Design goals, in order:

1. **Predictable for artists.** Turning a switch on should not silently change something unrelated. New options should default to a value that leaves the picture unchanged.
2. **Readable.** The code is deliberately repetitive: texture slots are written the same way so that one slot can be understood by reading another.
3. **Correct on edge cases** (zero widths, long play times, orthographic cameras, faded-out particles).

Performance tuning, variant stripping, and build size are explicitly **not** goals of this release. The shader uses `shader_feature_local` keywords throughout.

## 2. Files

| File | Role |
|---|---|
| `Shaders/VFXEssential.shader` | The shader `VFX/Essential`: properties, keywords, the `UnityPerMaterial` constant buffer, helper macros, `vert`, `frag`. |
| `Shaders/VFXEssentialFunctions.hlsl` | Shared functions: Custom Data lookup, UV transforms, Wrap Mode emulation, Diffuse channel selection, dissolve, tangent-frame reconstruction. |
| `Editor/VFXEssentialGUI.cs` | `VFXEssentialGUI : ShaderGUI`, the material Inspector, including the Vertex Streams check. The whole file is wrapped in `#if UNITY_EDITOR`. |
| `README.md`, `README_JA.md`, `README_ZH.md` | User documentation in English, Japanese, and Chinese. Keep all three in sync when behavior or UI changes. |
| `Documentation~/Images/*.png` | Inspector screenshots used by the README. |

When imported from the `.unitypackage`, the files live under `Assets/VFXEssentialShader/`.

## 3. How a pixel is built

### Vertex stage (`vert`)

1. **Collect per-vertex / per-particle inputs** into local variables: `vertexColor`, `cd1`, `cd2` (Custom Data), `uv1`, `uv2`. Where these come from depends on how the mesh is drawn (see [Vertex inputs and Mesh GPU Instancing](#vertex-inputs-and-mesh-gpu-instancing)). Everything after this point uses the locals and does not care about the source.
2. **Vertex Offset** (`_VERTEXOFFSETON_ON`): samples the offset texture at LOD 0, optionally multiplies by the Offset Mask, and moves the vertex in object space. If Distortion targets Vertex Offset or Offset Mask, the Distortion offset is evaluated here as well.
3. Outputs: clip position, `uv0 = (uv1, uv2)`, color, both Custom Data vectors, screen position, world normal, world position, and eye depth.

### Fragment stage (`frag`)

The order below is the documented composition order. Several effects sit where they do on purpose; do not reorder without understanding why.

1. **Distortion**: `DistortionOffset()` returns a UV offset. `NoiseIf(noise, targets, bit)` adds it to a slot's UV only if that slot's bit is set in `_NoiseTargets`.
2. **Diffuse 1**, then **Diffuse 2** blended into it. Result: `mainColor` (rgb = color, a = the textures' own alpha).
3. `gate = 1`, then **Dissolve** and **Mask** multiply into `gate`.
4. Scene depth is sampled once (`depthGap`) and the intersection-edge strength (`depthEdge`) is precomputed.
5. **Rim**: Fresnel with Output Channel A multiplies `gate`; Fresnel with Output Channel RGB only stores a mask for later; MatCap modifies `mainColor` and may produce `mcAddAlpha`.
6. **Final composite**: `rgb = mainColor.rgb * vertex color * Tint Color * Emission Scaler`; `gate *= vertex alpha * Tint alpha`; `alpha = saturate(mainColor.a + mcAddAlpha) * gate`.
7. Dissolve edge color in **Tint** mode (after Tint Color, so the edge color is not tinted).
8. **Fresnel RGB** (deferred for the same reason), suppressed where `depthEdge` is strong.
9. **Depth**: Surface Fade, or Edge Mode with Add / Multiply.
10. **Saturation** and **Contrast**. Contrast also changes alpha: for a non-negative value it pivots around 0.5 like RGB; for a negative value alpha is scaled toward 0 instead, so lowering Contrast never reveals transparent areas.
11. Edge Mode with **Tint** (after color grading, so the edge color is not graded). It can raise alpha again: `max(alpha, depthEdge * gate)`.
12. **Camera Fade** (last, so it fades everything including edge colors).

## 4. Modules and keywords

| Module (Inspector) | Switch property | Keywords | Requires |
|---|---|---|---|
| Custom Data | `_UseCustomData` | `_USECUSTOMDATA_ON` | |
| Diffuse 2 | `_DiffuseTex2On` | `_DIFFUSETEX2ON_ON`; `_DIFF2BLENDMODE_MULTIPLY`, `_DIFF2BLENDMODE_ADD` | |
| Mask | `_MaskTexOn` | `_MASKTEXON_ON` | |
| Dissolve | `_DissolveOn` | `_DISSOLVEON_ON`; `_DISSOLVEBLENDTYPE_ADD`, `_DISSOLVEBLENDTYPE_MULTIPLY`, `_DISSOLVEBLENDTYPE_TINT`, `_DISSOLVEBLENDTYPE_SELECT` | |
| Dissolve Mask | `_DissolveMaskOn` | `_DISSOLVEMASKON_ON` | Dissolve |
| Distortion | `_NoiseTexOn` | `_NOISETEXON_ON`; `_NOISEFLOWMODE_FLOWMAP` | |
| Distortion Mask | `_UseUVMask` | `_USEUVMASK_ON` | Distortion |
| Vertex Offset | `_VertexOffsetOn` | `_VERTEXOFFSETON_ON`; `_OFFSETDIRMODE_NORMAL`, `_OFFSETDIRMODE_WORLD`; `_AXISSCALEMODE_WORLD`, `_AXISSCALEMODE_LOCAL` | |
| Offset Mask | `_UseOffsetMask` | `_USEOFFSETMASK_ON` | Vertex Offset |
| Rim | `_RimMode` | `_RIMMODE_OFF`, `_RIMMODE_FRESNEL`, `_RIMMODE_MATCAP` | |
| Fresnel options | | `_FRESNELCHANNEL_RGB`, `_FRESNELCHANNEL_A`; `_FRESNELBLENDMODE_ADD`, `_FRESNELBLENDMODE_MULTIPLY`, `_FRESNELBLENDMODE_TINT`; `_INVERSE_ON` | Rim = Fresnel |
| MatCap options | | `_MATCAPBLENDMODE_MULTIPLY`, `_MATCAPBLENDMODE_ADD`; `_MATCAPCOLORMODE_RGB`, `_MATCAPCOLORMODE_RGBA`, `_MATCAPCOLORMODE_GRAYSCALE`, `_MATCAPCOLORMODE_RGBTOA`; `_MATCAPNORMALON_ON` | Rim = Matcap |
| Surface Fade | `_DepthOn` | `_DepthOn_On`; `_DEPTHMETHOD_SIMPLE`, `_DEPTHMETHOD_OPACITY_BASED` | Depth Texture in the URP Asset |
| Edge Mode | `_EdgeMode` | `_EDGEMODE_ON`; `_EDGEEFFECTCOLOR_ADD`, `_EDGEEFFECTCOLOR_MULTIPLY`, `_EDGEEFFECTCOLOR_TINT` | Surface Fade (`_DepthOn_On`) |
| Camera Fade | `_CameraFadeOn` | `_CAMERAFADE_ON` | Nothing; it uses the fragment's own eye depth, not the depth texture |
| Particle Mesh GPU Instancing | (set on the Renderer) | `PROCEDURAL_INSTANCING_ON` | Render Mode = Mesh |

- All feature switches are `shader_feature_local`. `[Toggle(KEYWORD)]` and `[KeywordEnum(...)]` properties generate the keyword names; a `KeywordEnum` property `_Foo` with value `Bar` uses `_FOO_BAR`.
- Code for an enum usually treats its first value as the `#else` branch, so a material with no keyword of that group set behaves as the default.

## 5. Conventions

### Texture slots

Most texture slots are written with one pattern:

```hlsl
int2   wm = WRAP_AXES(_Prefix);
float2 uv = SlotUV(SelUV(uv1, uv2, screenUV, _XxxUVMode) + NoiseIf(noise, noiseTargets, BIT),
                   _XxxAngle + SpinDeg(_XxxAngleSpeed * _XxxAngleSpeedOn) + CD(_XxxAngleCoord),
                   _XxxTex_ST,
                   ScrollOffset(_XxxSpeed.xy * _XxxOffsetSpeedOn, wm)
                   + float2(CD(_XxxOffsetXCoord), CD(_XxxOffsetYCoord)));
float4 s = SAMPLE_SLOT(_XxxTex, uv, wm);
```

- UV order: choose the UV set, add Distortion, rotate around (0.5, 0.5), apply Tiling/Offset, then add scrolling and Custom Data offsets.
- Distortion is added in the shared base-UV space, **before** each slot's own rotation and Tiling. The warp is therefore the same on the mesh surface for every targeted slot, whatever its rotation or Tiling. (In a slot's own texture space the offset is of course rotated and scaled with it.) Do not move the addition after `SlotUV`.
- Two slots deliberately differ from the pattern:
  - **MatCap texture**: its UV comes from the view-space reflection vector, not from a UV set. Tiling/Offset is applied first and rotation second. No Distortion and no scrolling.
  - **MatCap Normal Map**: UV set, Tiling/Offset, scrolling, and Custom Data offsets, but no rotation and no Distortion.
- **Rule:** `WRAP_AXES(prefix)` builds four property names by token pasting: `prefix##WrapMode`, `prefix##WrapModeU`, `prefix##WrapModeV`, `prefix##WrapOn`. The four wrap properties of a slot must share one prefix. Existing prefixes: `_Diff1`, `_Diff2`, `_DissTex`, `_DissMask`, `_Mask`, `_Noise`, `_NoiseUVMask`, `_VtxOffset`, `_VtxMask`, `_MatCap`, `_MatCapNormal`.
- `SAMPLE_SLOT(tex, uv, wm)` expects `sampler##tex` and `tex##_TexelSize` to exist. Use `SAMPLE_SLOT_LOD0` in the vertex stage, and `SAMPLE_SLOT_STAGE` in code compiled for both stages.
- Single-channel slots read `sample[(int)_XxxChannel]` with 0–3 = R, G, B, A. The default is 1 (G).

### Custom Data is always additive

**Rule:** every Custom Data binding is written as `material value + CD(_XxxCoord)`. Never write a binding that *replaces* the material value. With additive bindings, enabling Custom Data while the Particle System supplies zeros changes nothing on screen, and users rely on that.

- A coord property holds 0 = None, 1–4 = Custom1.xyzw, 5–8 = Custom2.xyzw. `CD()` returns 0 when the coord is None or when Custom Data is disabled.
- `CD()` is a macro that reads local variables named `cd1` and `cd2`. `vert` and `frag` declare them at the top; a helper function that uses `CD()` must take them as parameters with exactly those names (see `DistortionOffset`).
- Clamping after the addition is **not** uniform. Do not add or remove clamps on existing bindings; it changes how existing materials look.

| Value | Default binding | After the addition |
|---|---|---|
| U / V offset of a slot | Diffuse Tex 1: CD1.x, CD1.y. Mask Tex: CD1.z, CD1.w. Others: None | Added to the UV offset, in UV units |
| Angle of a slot, MatCap Angle | None | Added, in degrees |
| Dissolve Range | CD2.x | Not clamped |
| Smooth Intensity | CD2.y | `saturate` |
| Dissolve Edge Width | None | Not clamped |
| Dissolve Mask Intensity | None | Not clamped |
| Distortion Intensity | CD2.z | Not clamped |
| Vertex Offset Intensity | CD2.w | Not clamped |
| Diffuse 2 Intensity, Mask Intensity, Distortion Mask Intensity, Offset Mask Intensity, MatCap Intensity | None | `saturate` |
| Fresnel Progress, Fresnel Sharpness | None | Upper bound only: `min(1, …)` |
| Fresnel Intensity | None | Not clamped |

Angle Speed is in turns per second.

### Alpha goes through `gate`

**Rule:**

- `mainColor.a` holds only alpha that comes from textures.
- Everything that *hides* the surface (Dissolve, Mask, Fresnel A, vertex alpha, Tint alpha) multiplies `gate`.
- Final alpha is `saturate(mainColor.a + added alpha) * gate`.
- Any effect that *adds* alpha or takes a `max` (MatCap Add, intersection edges) must multiply its contribution by `gate`. Otherwise a particle that has faded out or dissolved still shows an edge line.

### Masks do not all work the same way

| Mask | Formula | Neutral at |
|---|---|---|
| Mask, Distortion Mask, Offset Mask | `x *= lerp(1, sample, intensity)` | Intensity 0 |
| Dissolve Mask | `dissolveRange -= sample * intensity` (brighter areas dissolve later). It shifts the threshold; it does not multiply alpha. | Intensity 0 |

Dissolve blend modes: Add and Multiply apply the edge color and multiply `gate` by the dissolve alpha. **Tint** stores the edge coverage for step 7 and reduces `gate` by the edge band scaled with `1 - Edge Color alpha`. **Select** keeps only the edge band (`gate *= edge`) and ignores Edge Color; with Edge Width 0 nothing remains visible.

### No zero-width smoothstep

**Rule:** `smoothstep(a, b, x)` with `a == b` is undefined and produces NaN on some mobile GPUs, which bloom then spreads into blocks. Dissolve with Smooth Intensity 0 and Fresnel with Sharpness 1 both hit this. Always clamp the width: `max(width, 1e-4)`.

### Wrap Mode emulation

Wrap Mode in the material overrides the texture's own wrap setting without duplicating the texture.

- Values: 0 = None (follow the texture), 1 = Repeat, 2 = Clamp, 3 = Mirror, 4 = Mirror Once, 5 = Per-axis. `_XxxWrapOn` is the master switch; off means None.
- **Precondition:** textures are imported with Wrap Mode = Repeat. Repeat and None hand the UV to the hardware sampler unchanged. Clamp, Mirror, and Mirror Once fold the UV into 0–1 with continuous math and pull it away from the border by an inset, then sample normally. In the fragment stage the inset is estimated from the UV footprint (it grows as the texture is minified); the LOD variant uses `0.5 * exp2(lod)` texels.
- **Rule:** do not switch to `SAMPLE_TEXTURE2D_GRAD`, and do not apply `frac` to the UV on a Repeat axis. Both were tried and removed (seams, and bleeding from the opposite border when minified).
- Scrolling goes through `ScrollOffset()`. For Repeat and Mirror it wraps the accumulated offset to the wrap period, which is what prevents texture breakup after long play times. For None, Clamp, and Mirror Once the offset is left as `_Time.y * speed`, because wrapping it would change the picture. Rotation uses `SpinDeg()`, which takes `frac` before multiplying by 360. Do not replace either with raw time expressions.

### Diffuse Texture Channel and blending

`DiffuseChannel()` splits a sample into color and alpha and reports which of the two the layer provides (`hasColor`, `hasAlpha`). Modes: 0 = RGBA, 1 = RGB, 2 = R (grayscale color), 3 = A, 4 = R2A (R used as alpha).

**Rule:** an output a layer does not provide takes the neutral value of the blend, 1 for Multiply and 0 for Add, on both layers. If neither layer provides an output, it stays 1 (white / opaque); without this, Add would produce an invisible surface. With Diffuse 2 disabled, missing outputs of Diffuse 1 are 1.

With `c`, `a` the outputs of each layer and `i` the Diffuse 2 Intensity:

- Multiply: `rgb = c1 * lerp(1, c2, i)`, `alpha = a1 * lerp(1, a2, i)`, where a missing output is 1.
- Add: `rgb = c1 + c2 * w * i`, `alpha = saturate(a1 + a2 * i)`, where a missing output is 0 and `w` is `a2` if layer 2 provides alpha, otherwise 1.

Example: Diffuse 1 = RGB and Diffuse 2 = A gives color from layer 1 and alpha from layer 2 in both modes.

### Emission Scaler

There are three (Render Options, Diffuse 2, MatCap). All are a direct brightness multiplier with range 1–50 and default 1. Keep new brightness controls consistent with this.

### Distortion targets

`_NoiseTargets` is a bit mask: 1 Diffuse 1, 2 Diffuse 2, 4 Mask, 8 Dissolve, 16 Dissolve Mask, 32 Vertex Offset (its Offset Tex), 64 Offset Mask. −1 means all and is the default. The enum `NoiseTarget` in the Inspector script must match.

`DistortionOffset()` is shared by both stages. The fragment stage always evaluates it when Distortion is on; the vertex stage evaluates it only when bit 32 or 64 is set and Vertex Offset is enabled.

### Vertex Offset

- Direction = Normal: the offset follows the vertex normal; Axis Scale scales it per axis; Scale Mode chooses whether those axes are world or object axes. Scale Mode is only meaningful (and only shown) for Normal.
- Direction = World: Axis Scale **is** the direction vector, in world space, and is not normalized.
- Screen UV in the vertex stage is computed from the position **before** the offset; the fragment stage uses the position after it. A texture in Screen mode is therefore not sampled at exactly the same place in both stages.
- Normals are not recomputed after the offset, so Rim uses the original normals. Renderer bounds are not enlarged either; a large offset can be culled early.

### Depth

- `SceneEyeDepth()` handles both perspective and orthographic cameras.
- There are two softness computations, on purpose. `depthEdge` (used by Edge Mode Tint and to suppress Fresnel RGB) is computed early from `_DepthSoftness` alone. Surface Fade and Edge Mode Add / Multiply compute their own factor later, and with Depth Method = Opacity Based they stretch the softness by up to 8× for transparent pixels. Do not merge the two without checking each mode.

### Saturation uses two sets of luminance weights

Global Saturation uses Rec. 709 weights (0.2126, 0.7152, 0.0722); MatCap's own Saturation and Grayscale use (0.299, 0.587, 0.114). Unifying them changes existing looks.

### MatCap normal map needs no tangents

`TangentToWorldNoTangent()` rebuilds the tangent frame per pixel from screen-space derivatives of world position and of `normalBaseUV`, which is the selected UV set multiplied by the normal map's Tiling. Offset, scrolling, and Custom Data offsets are left out because they are constant across a particle and do not affect the derivatives. If you add rotation or any other non-uniform transform to this slot, pass the transformed UV as well.

**Rule:** the shader has no `TANGENT` input, and none should be added. When a Tangent stream is missing, Unity silently supplies a constant (1, 0, 0), which only *looks* right for unrotated, camera-aligned quads.

### Vertex inputs and Mesh GPU Instancing

The shader reads fixed semantics. With Custom Data enabled, the layout is:

| Input | Semantic |
|---|---|
| UV (xy) and UV2 (zw) | `TEXCOORD0` |
| Custom1.xyzw | `TEXCOORD1` |
| Custom2.xyzw | `TEXCOORD2` |

Without Custom Data, `TEXCOORD1` carries the mesh's second UV set instead. Where UV2 comes from is decided by keywords, not by the renderer type:

- `_USECUSTOMDATA_ON` set: UV2 = `TEXCOORD0.zw`. A Particle System must use the Custom Vertex Streams `Position, Normal, Color, UV, UV2, Custom1.xyzw, Custom2.xyzw` (the TEXCOORD-packed ones in exactly this order). A Mesh Renderer with Custom Data enabled would need the same layout in its mesh, which is unusual; keep Custom Data off on Mesh Renderers.
- `_USECUSTOMDATA_ON` not set: UV2 = `TEXCOORD1.xy`, the mesh's second UV set.
- Particle Mesh GPU Instancing: UV2 = `TEXCOORD1.xy` in both cases (see below).

**Particle System Mesh GPU Instancing** (Render Mode = Mesh with *Enable Mesh GPU Instancing* on the Renderer) is supported through `#pragma instancing_options procedural:ParticleInstancingSetup`. It turns on automatically. The material's own *Enable GPU Instancing* flag has no effect on particles and is not shown in the Inspector.

- In this mode Unity draws the mesh's **own** vertex data. Per-particle values are not vertex attributes; they are read from `unity_ParticleInstanceData[unity_InstanceID]`.
- **Rule:** the struct layout must equal the order of the per-particle streams in the Renderer's stream list, with no extra per-particle streams. Without Custom Data the shader uses URP's default struct (`transform, color, animFrame`), matching the streams `Color, AnimFrame`. With Custom Data it defines `VFXParticleInstanceData` (`transform, color, custom1, custom2, animFrame`), matching `Color, Custom1.xyzw, Custom2.xyzw, AnimFrame`.
- Color is multiplied by the mesh's vertex color when the mesh has one (`unity_ParticleUseMeshColors`), and Texture Sheet Animation is applied to UV1 in the shader from `animFrame` and `unity_ParticleUVShiftData`.
- A mismatch produces no error, only garbage. The Inspector's Vertex Streams check (`RequiredStreams`, `RequiredInstancedStreams`, `DrawVertexStreamCheck`) detects it and offers a one-click fix. **If you change the vertex inputs or the instance struct, change those functions too.**

### The constant buffer

**Rule:** every numeric material property that the HLSL reads, including each texture's `_ST` and `_TexelSize`, is declared inside `CBUFFER_START(UnityPerMaterial)`. Texture objects and samplers are declared outside it (`TEXTURE2D(_Tex); SAMPLER(sampler_Tex);`). The shader is SRP Batcher compatible and should stay that way.

## 6. Inspector name to code name

The Inspector uses artist-facing names; several differ from the property names.

| Inspector | Code |
|---|---|
| Distortion | `_Noise*`, keyword `_NOISETEXON_ON` |
| Distortion Mask | `_NoiseUVMask*`, keyword `_USEUVMASK_ON` |
| Distortion type Flow Map | keyword `_NOISEFLOWMODE_FLOWMAP` |
| Mid Point (Distortion) | `_NoiseOffset` |
| Mid Point (Vertex Offset) | `_VertexOffsetBaseValue` |
| Offset Tex / Offset Mask | `_VertexOffsetTex` / `_VertexOffsetMaskTex` |
| Offset Intensity | `_VertexOffsetIntensityCustom2w` |
| Axis Scale / Scale Mode | `_OffsetDirectionXYZ` / `_AxisScaleMode` |
| Tint Color | `_ColorRGBA` |
| Blend Mode (Render Options) | `_DstBlendMode` (the pass is `Blend SrcAlpha [_DstBlendMode]`) |
| Facing | `_CullMode` |
| Always On Top | `_AlwaysOnTop` (a `ZTest` value) |
| Enable Custom Data | `_UseCustomData`, keyword `_USECUSTOMDATA_ON` |
| CD Index | `_Xxx*Coord` |
| Dissolve Range / Smooth Intensity | `_DissolveRangeCustom2x` / `_SmoothIntensityCustom2y` |
| Surface Fade | `_DepthOn`, keyword `_DepthOn_On` |

## 7. The Inspector script

- `FindProperties()` looks up every property into a `p_*` field. A property that is missing from the shader throws here, so shader and script must be edited together.
- `OnGUI()` draws nine sections in order: Render Options, Diffuse 1, Diffuse 2, Mask, Dissolve, Distortion, Vertex Offset, Rim, Depth. Each has a static `_XxxFoldout` flag and uses `Foldout()` for its header.
- Reusable drawers: `DrawTextureBlock` (texture, Tiling, Offset, UV Options), `DrawWrapModeRow`, `DrawDiffuseChannelRow`, `DrawTexUVOptionsBox` (CD Index, Offset Speed, Angle, Angle Speed), `DrawAngleWithCoord`, `DrawToggleLeft` / `DrawToggleRight`.
- Always register undo (`RegisterPropertyChangeUndo`) and respect `hasMixedValue`; the existing rows show the pattern. Sections decide what to show from the first selected material, so multi-selection of materials with different modes is only partly supported.

### State the script manages

A material's float properties, its keywords, and the Inspector's side effects are three separate things. The shader does not derive keywords from floats at runtime.

- **Keywords.** If you change a toggle or enum from a script, set the float **and** the keyword. Within a group, disable the other keywords as well:

  ```csharp
  material.SetFloat("_Diff2BlendMode", 1f);
  material.DisableKeyword("_DIFF2BLENDMODE_MULTIPLY");
  material.EnableKeyword("_DIFF2BLENDMODE_ADD");
  ```

- **Render queue.** Render Priority writes `material.renderQueue = 3000 + value`, but only when the value is changed in the Inspector.
- **Facing lock.** Switching Rim to Matcap forces Facing to Front and stores the previous value in `_PreMatCapCullMode`; leaving Matcap restores it. −1 means nothing is stored.
- **Always On Top.** Turning it on disables Surface Fade (and with it Edge Mode) and stores its state in `_PreAOTDepthOn`; turning it off restores it. −1 means nothing is stored. Camera Fade is not affected.
- **Stale keywords.** When a mode changes, the script clears keywords of the modes that were left with `DisableKeywords()`. If you add or remove an enum value, update those lists.

### Custom Data bookkeeping

Two functions keep their own list of bindings: `BuildCDUsers()` (finds channels used by more than one parameter and colors them red) and `DrawCustomDataSummary()` (draws the Custom Data Map). A new binding must be registered in **both**, with the same "is this module active" condition. A shared channel is a warning, not an error; driving two parameters from one channel is sometimes intended.

### Vertex Streams check

- It scans loaded scenes (inactive objects included) for Particle System Renderers whose shared material is the first selected material, at most once per second. It does not look at prefabs that are not in a scene.
- Non-instanced renderers are checked only when Custom Data is enabled. Renderers that actually render with Mesh GPU Instancing (`renderMode == Mesh`, `enableGPUInstancing`, and `supportsMeshInstancing`) are always checked.
- Fixing a non-instanced renderer puts the required streams first and keeps the others after them. Fixing an instanced renderer rebuilds the list (`Position, Normal, Color, UV, UV2`, then the required per-particle streams) and drops everything else, because an extra per-particle stream would shift the layout.
- `ParticleSystemRenderer.GetActiveVertexStreams()` returns the last stored list even when *Custom Vertex Streams* is unchecked, so the script reads the serialized `m_UseCustomVertexStreams` flag separately.

## 8. Common tasks

### Add a texture slot

1. Properties: the texture, `_XxxSpeed`, `_XxxOffsetXCoord` / `_XxxOffsetYCoord`, `_XxxUVMode`, `_XxxAngle` / `_XxxAngleCoord` / `_XxxAngleSpeed`, `_XxxOffsetSpeedOn` / `_XxxAngleSpeedOn`, a channel property if single-channel, and the four wrap properties sharing one new prefix.
2. Declare `TEXTURE2D` and `SAMPLER` outside the constant buffer; add `_XxxTex_ST`, `_XxxTex_TexelSize`, and every numeric property inside it.
3. Write the slot using the pattern in [Texture slots](#texture-slots). If it should be a Distortion target, add a bit, extend `NoiseTarget`, and widen the mask the Inspector applies to `_NoiseTargets`.
4. Inspector: add `p_*` fields, look them up in `FindProperties()`, and draw them with the existing drawers.
5. Update the three READMEs and re-capture the affected screenshot.

### Add a Custom Data-driven parameter

Add a `_XxxCoord` float (default 0), add it to the constant buffer, write `value + CD(_XxxCoord)`, draw the CD dropdown next to the control, and register it in both `BuildCDUsers()` and `DrawCustomDataSummary()`.

### Change an enum

Changing the order or removing a value changes the stored index of existing materials. Either append new values at the end, or migrate existing materials (both the float and the keywords). Update the Inspector's `DisableKeywords()` lists and any documentation tables.

## 9. How to verify a change

There is no automated test suite in this repository. The checks below are the ones used during development, run from Editor scripts in Unity 6000.0 with URP 17.0 on Direct3D 11.

- **Compile variants explicitly.** A material only compiles the variant it uses, so an error in another keyword combination stays hidden. Compile representative combinations for both stages, including `INSTANCING_ON` and `PROCEDURAL_INSTANCING_ON`, and read the messages:

  ```csharp
  var pass   = ShaderUtil.GetShaderData(shader).GetSubshader(0).GetPass(0);
  var result = pass.CompileVariant(ShaderType.Fragment,
                                   new[] { "_DISSOLVEON_ON", "_DISSOLVEBLENDTYPE_TINT" },
                                   ShaderCompilerPlatform.D3D, BuildTarget.StandaloneWindows64);
  // result.Success, result.Messages
  ```

- **Check blend math numerically.** Draw a quad with 1×1 textures of known colors into a linear floating-point render texture and compare the pixels with values computed independently. The pass blends with `SrcAlpha` and does not write alpha (`ColorMask RGB`), so with Blend Mode = AlphaBlend render once on black and once on white: `alpha = 1 - (white - black)` and `rgb = black / alpha`. This does not work with Blend Mode = Add, or where alpha is 0.
- **Compare instanced and non-instanced particles.** Render the same Particle System (Mesh mode, fixed random seed) with *Enable Mesh GPU Instancing* off and on, and compare the images. Use particles that differ in color, Custom Data, rotation, and animation frame so that a wrong layout is obvious.
- **Cases worth keeping:**
  - every alpha-adding mode combined with `gate` = 0 (nothing may remain visible);
  - Custom Data enabled with all-zero data against Custom Data disabled (identical picture);
  - Diffuse layers that provide only color or only alpha, in both blend modes, and Diffuse 2 Intensity = 0;
  - Rim to Matcap and back, Always On Top on and off, with undo;
  - zero widths (Smooth Intensity 0, Fresnel Sharpness 1), an orthographic camera, and a strongly minified Clamp or Mirror texture.
- **Check SRP Batcher compatibility** after adding properties.

## 10. Pitfalls

- **Legacy property names.** `_DissolveRangeCustom2x`, `_SmoothIntensityCustom2y`, `_NoiseIntensityCustom2z`, `_VertexOffsetIntensityCustom2w` are named after the Custom Data channel they were once hard-wired to. They are ordinary material values now (their `*Coord` properties default to those channels). Do not rename them: existing materials reference the names.
- **`_DepthOn_On` is mixed case.** Keep it exactly as is.
- **Default enum keywords.** In Unity 6000.0, `new Material(shader)` in the Editor comes with the default keyword of each `KeywordEnum` already enabled. Enabling a second keyword of the same group without disabling the first leaves both set, and which variant is used is then not something to rely on.
- **`ColorMask RGB`.** The pass does not write alpha to the target.
- **Derivatives.** `SAMPLE_SLOT` and `TangentToWorldNoTangent` rely on screen-space derivatives and are fragment-only. Use the LOD variants in `vert`.
- **Texture Sheet Animation under Mesh GPU Instancing** affects UV1 only. Without instancing Unity applies it to every UV channel selected in the module.
- **Particles are already in world space** when they are not instanced, so "object space" in `vert` equals world space there. With Mesh GPU Instancing, object space is the particle's own space.
- **Enabling a module is not always visually neutral.** For example, Distortion defaults to Intensity 1 with Mid Point 0.5, so a default white texture already offsets the UV.
- **Documentation is part of the change.** UI labels appear verbatim in three READMEs and in screenshots.

## 11. Known limitations

- No fog.
- No frame blending for Texture Sheet Animation.
- No support for UI `Mask` / `RectMask2D`.
- No XR Single Pass Instanced rendering.
- At Dissolve Range 0 the darkest values of the dissolve texture already fall inside the transition (its width is Smooth Intensity, never less than `1e-4`) or inside the edge band (Edge Width). This is by design; users start from a negative range.
- Keyword-based and not optimized for variant count, build time or size, or runtime performance.
