#ifndef VFX_ESSENTIAL_FUNCTIONS_INCLUDED
#define VFX_ESSENTIAL_FUNCTIONS_INCLUDED

// ============================================================
//  Custom Data
//  coord: 0 = unassigned (returns 0), 1-4 = Custom1.xyzw, 5-8 = Custom2.xyzw
// ============================================================
float GetCoordValue(float coord, float4 cd1, float4 cd2)
{
    int c = (int)coord;
    if (c <= 0) return 0.0;
    c--;
    float4 cd = c < 4 ? cd1 : cd2;
    int    ch = c & 3;   // = c % 4 (c is now >= 0); bitwise operations are cheaper than integer modulo.
    return ch == 0 ? cd.x : ch == 1 ? cd.y : ch == 2 ? cd.z : cd.w;
}

// ============================================================
//  UV utilities
// ============================================================

// UV Options selection: 0 = UV1, 1 = UV2, 2 = Screen.
float2 SelUV(float2 uv1, float2 uv2, float2 screenUV, float mode)
{
    return mode < 0.5 ? uv1 : mode < 1.5 ? uv2 : screenUV;
}

// Rotate UVs around (0.5, 0.5).
float2 RotateUV(float2 uv, float angleDeg)
{
    float s, c;
    sincos(radians(angleDeg), s, c);
    float2 p = uv - 0.5;
    return float2(c * p.x - s * p.y, s * p.x + c * p.y) + 0.5;
}

// Complete UV transform for one texture slot: rotation -> Tiling / Offset -> UV scroll and Custom Data offsets.
float2 SlotUV(float2 uv, float angleDeg, float4 st, float2 offset)
{
    return RotateUV(uv, angleDeg) * st.xy + st.zw + offset;
}

// Angle Speed rotation in degrees; turnsPerSecond is the number of revolutions per second.
// Take the fractional part before multiplying by 360: full turns are equivalent to no rotation, avoiding angle jitter from precision loss as _Time increases.
float SpinDeg(float turnsPerSecond)
{
    return frac(_Time.y * turnsPerSecond) * 360.0;
}

// Apply Distortion only to selected targets (bit corresponds to the Inspector's Distortion Targets).
float2 NoiseIf(float2 noise, int targets, int bit)
{
    return (targets & bit) != 0 ? noise : float2(0.0, 0.0);
}

// ============================================================
//  Texture Channel for Diffuse textures
//  mode: 0 = RGBA  1 = RGB  2 = R  3 = A  4 = R2A
//
//  A texture layer can provide color, Alpha, or both:
//    RGBA -> color = RGB, Alpha = A
//    RGB  -> color only
//    R    -> color only (grayscale from the R channel)
//    A    -> Alpha only (A channel)
//    R2A  -> Alpha only (from the R channel)
//  An unused output leaves the blend result unchanged (see Diffuse 1 / Diffuse 2 in the shader).
// ============================================================
void DiffuseChannel(half4 s, float mode, out half3 color, out half alpha, out half hasColor, out half hasAlpha)
{
    int m = (int)mode;
    color    = m == 2 ? s.rrr : s.rgb;
    alpha    = m == 4 ? s.r : s.a;
    hasColor = m <= 2 ? 1.0h : 0.0h;
    hasAlpha = (m == 0 || m >= 3) ? 1.0h : 0.0h;
}

// ============================================================
//  Tangent-space normal -> world-space normal (no vertex tangents required)
//
//  normalTS - tangent-space normal decoded from the normal map
//  N        - world-space normal (normalized)
//  p        - world-space position
//  uv       - UVs used to sample the normal map
//
//  Use screen-space derivatives to find the surface directions of increasing U and V as the tangent and bitangent.
//  Particles need no additional Tangent Vertex Stream; directions remain correct for any particle rotation or texture UV set.
//  Call only in the fragment stage.
// ============================================================
float3 TangentToWorldNoTangent(float3 normalTS, float3 N, float3 p, float2 uv)
{
    float3 dp1  = ddx(p),  dp2  = ddy(p);
    float2 duv1 = ddx(uv), duv2 = ddy(uv);

    float3 dp2perp = cross(dp2, N);
    float3 dp1perp = cross(N, dp1);
    // The determinant sign depends on screen-coordinate orientation and face orientation; restore it to prevent flipped directions.
    float  s = dot(dp1, dp2perp) < 0.0 ? -1.0 : 1.0;
    float3 T = (dp2perp * duv1.x + dp1perp * duv2.x) * s;
    float3 B = (dp2perp * duv1.y + dp1perp * duv2.y) * s;

    // If UVs do not vary across the screen, directions cannot be derived; skip perturbation and use the original normal.
    float lenT2 = dot(T, T), lenB2 = dot(B, B);
    float valid = min(lenT2, lenB2) > 1e-30 ? 1.0 : 0.0;
    T *= rsqrt(max(lenT2, 1e-30)) * valid;
    B *= rsqrt(max(lenB2, 1e-30)) * valid;
    return normalize(normalTS.x * T + normalTS.y * B + normalTS.z * N);
}

// ============================================================
//  Dissolve + edge highlight
//
//  diss             - Dissolve value (a single grayscale value from the selected channel)
//  dissRange        - Dissolve threshold; higher values dissolve a larger region
//  smoothness       - transition width at the Dissolve boundary
//  edgeRange        - width of the edge highlight band
//  outDissolveAlpha - main Dissolve Alpha
//  outEdgeAlpha     - edge highlight mask
// ============================================================
void HardDissolveEdges(float diss, float dissRange, float smoothness, float edgeRange,
                       out float outDissolveAlpha, out float outEdgeAlpha)
{
    // A zero-width smoothstep divides by zero (undefined behavior; may cause NaN pixel artifacts on mobile), so use a minimal width.
    float w = max(smoothness, 1e-4);

    outDissolveAlpha = smoothstep(dissRange, dissRange + w, diss);

    float ss2 = smoothstep(dissRange + edgeRange, dissRange + edgeRange + w, diss);
    outEdgeAlpha = saturate(outDissolveAlpha - ss2);
}

// ============================================================
//  UV Wrap Mode (override the texture's Wrap Mode per material)
//
//  Requirement: all textures use Wrap Mode = Repeat in their Import Settings.
//  Main mode: 0 = None (use Import Settings)  1 = Repeat  2 = Clamp  3 = Mirror  4 = Mirror Once
//          5 = Per-axis (U / V each use 0-4)
//
//    None / Repeat                -> pass UVs unchanged to the hardware Repeat sampler
//    Clamp / Mirror / Mirror Once -> mathematically fold UVs into [0,1], then inset them slightly
//                                  to prevent bilinear filtering from wrapping across the boundary to the opposite edge
//
//  The folded UVs are continuous (no frac discontinuities), so regular sampling is sufficient:
//  the texture's own sampler handles mip selection and anisotropic filtering.
// ============================================================

// (main mode, U, V, master toggle) -> effective mode for each axis.
int2 WrapAxes(float mainMode, float modeU, float modeV, float wrapOn)
{
    int m = wrapOn < 0.5 ? 0 : (int)mainMode;   // Master toggle off -> None.
    return m == 5 ? int2((int)modeU, (int)modeV) : int2(m, m);
}

// Protect UV scroll offsets against precision loss.
// As _Time increases, adding time * speed directly to UVs loses fractional UV precision, causing blocky artifacts.
// Wrap the offset by the texture's repeat period first (Repeat: 1, Mirror: 2) to keep UV values small.
// Clamp / Mirror Once have no period, so do not wrap; None has an unknown texture Wrap Mode, so do not wrap either.
float ScrollWrap1D(float v, int m)
{
    return m == 1 ? frac(v) : (m == 3 ? frac(v * 0.5) * 2.0 : v);
}

float2 ScrollOffset(float2 speed, int2 wm)
{
    float2 o = _Time.y * speed;
    return float2(ScrollWrap1D(o.x, wm.x), ScrollWrap1D(o.y, wm.y));
}

// Fold one UV axis into [inset, 1 - inset] according to its Wrap Mode.
float WrapCoord1D(float v, int m, float inset)
{
    float s = m == 2 ? v                                      // Clamp
            : m == 3 ? 1.0 - abs(frac(v * 0.5) * 2.0 - 1.0)   // Mirror (triangle wave)
            :          abs(v);                                // MirrorOnce
    return (m >= 2 && m <= 4) ? clamp(s, inset, 1.0 - inset) : v;   // None / Repeat: unchanged.
}

float2 WrapCoord(float2 uv, int2 wm, float2 inset)
{
    return float2(WrapCoord1D(uv.x, wm.x, inset.x), WrapCoord1D(uv.y, wm.y, inset.y));
}

// Fragment stage sampling. texelSize = the texture's _TexelSize (xy = 1/width, 1/height; zw = width, height).
float4 SampleWrap2D(TEXTURE2D_PARAM(tex, smp), float2 uv, int2 wm, float4 texelSize)
{
    // Inset amount. A minified texture is sampled from a lower-resolution mip, whose texels are larger;
    // compute the inset for that mip, otherwise bilinear filtering can still wrap to the opposite edge's color.
    float2 dx = ddx(uv) * texelSize.zw;   // Texels covered by each screen pixel.
    float2 dy = ddy(uv) * texelSize.zw;
    float  texelsPerPixel = sqrt(max(dot(dx, dx), dot(dy, dy)));
    float2 inset = min(max(0.5, texelsPerPixel) * texelSize.xy, 0.5);

    return SAMPLE_TEXTURE2D(tex, smp, WrapCoord(uv, wm, inset));
}

// Vertex stage sampling (no screen-space derivatives; fixed LOD).
float4 SampleWrap2DLod(TEXTURE2D_PARAM(tex, smp), float2 uv, float lod, int2 wm, float4 texelSize)
{
    float2 inset = min(0.5 * exp2(lod) * texelSize.xy, 0.5);
    return SAMPLE_TEXTURE2D_LOD(tex, smp, WrapCoord(uv, wm, inset), lod);
}

#endif // VFX_ESSENTIAL_FUNCTIONS_INCLUDED
