Shader "VFX/Essential"
{
    Properties
    {
        [IntRange] _RenderPriority ("Render Priority", Range(-50, 50)) = 0
        [Enum(Off, 4, On, 0)]                    _AlwaysOnTop  ("Always On Top",   Float) = 4
        [Enum(Double, 0, Front, 2, Back, 1)]     _CullMode     ("Facing",           Int)   = 0
        [Enum(Add, 1, AlphaBlend, 10)]           _DstBlendMode ("Dst Blend Mode",  Float) = 10
        [Enum(Off, 0, On, 1)]                    _ZWriteMode   ("Z Write Mode",    Float) = 0
        [Toggle(_USECUSTOMDATA_ON)]              _UseCustomData("=== Use Custom Data ===", Float) = 0
        _Saturation        ("Saturation",              Range(-1, 1)) = 0
        _Contrast          ("Contrast",                Range(-1, 1)) = 0

        _DiffuseTex1       ("Diffuse Tex 1",          2D)     = "white" {}
        _EmissionScaler    ("Emission Scaler",        Range(1, 50)) = 1
        _ColorRGBA         ("Tint Color",             Color)  = (1,1,1,1)
        _DiffuseTex1Speed  ("Diffuse Tex1 Speed(UV)", Vector) = (0,0,0,0)
        _Diff1OffsetXCoord ("Diff1 CD Offset X",      Float)  = 1
        _Diff1OffsetYCoord ("Diff1 CD Offset Y",      Float)  = 2
        [Enum(UV1, 0, UV2, 1, Screen, 2)] _Diff1UVMode ("Diff1 UV Mode", Float) = 0
        _Diff1ChannelMode    ("Diff1 Texture Channel", Float)    = 0
        _Diff1Angle          ("Diff1 Angle",       Range(0,360)) = 0
        _Diff1AngleCoord     ("Diff1 Angle CD",    Float)        = 0
        _Diff1AngleSpeed     ("Diff1 Angle Speed", Float)        = 0

        [Toggle(_DIFFUSETEX2ON_ON)] _DiffuseTex2On("=== DiffuseTex2 On ===", Float) = 0
        [KeywordEnum(Multiply, Add)] _Diff2BlendMode ("Diff2 Blend Mode", Float) = 0
        _DiffuseTex2       ("DiffuseTex2",             2D)     = "white" {}
        _DiffuseTex2Speed  ("Diffuse Tex2 Speed(UV)",  Vector) = (0,0,0,0)
        _Diff2OffsetXCoord ("Diff2 CD Offset X",       Float)  = 0
        _Diff2OffsetYCoord ("Diff2 CD Offset Y",       Float)  = 0
        [Enum(UV1, 0, UV2, 1, Screen, 2)] _Diff2UVMode ("Diff2 UV Mode", Float) = 0
        _Diff2ChannelMode    ("Diff2 Texture Channel", Float)    = 0
        _Diff2Angle          ("Diff2 Angle",       Range(0,360)) = 0
        _Diff2AngleCoord     ("Diff2 Angle CD",    Float)        = 0
        _Diff2AngleSpeed     ("Diff2 Angle Speed", Float)        = 0
        _Diff2EmissionScaler ("Diff2 Emission Scaler", Range(1, 50)) = 1
        _Diff2Intensity      ("Diff2 Intensity",    Range(0, 1)) = 1
        _Diff2IntensityCoord ("Diff2 Intensity CD", Float)        = 0

        [Toggle(_DISSOLVEON_ON)] _DissolveOn("=== Dissolve On ===", Float) = 0
        _DissolveTex              ("Dissolve Tex",             2D)            = "white" {}
        _DissSpeed                ("Dissolve Speed(UV)",       Vector)        = (0,0,0,0)
        _DissTexOffsetXCoord      ("DissTex CD Offset X",      Float)         = 0
        _DissTexOffsetYCoord      ("DissTex CD Offset Y",      Float)         = 0
        [Enum(UV1, 0, UV2, 1, Screen, 2)] _DissTexUVMode ("DissTex UV Mode", Float) = 0
        _DissTexAngle          ("DissTex Angle",       Range(0,360)) = 0
        _DissTexAngleCoord     ("DissTex Angle CD",    Float)        = 0
        _DissTexAngleSpeed     ("DissTex Angle Speed", Float)        = 0
        _DissolveTexChannel       ("Dissolve Tex Channel", Float)    = 1
        _DissolveRangeCustom2x    ("Dissolve Range",    Float)      = 0
        _DissolveRangeCoord       ("Dissolve Range CD", Float)      = 5
        _SmoothIntensityCustom2y  ("Smooth Intensity",  Range(0,1)) = 0
        _SmoothIntensityCoord     ("Smooth Intensity CD", Float)    = 6
        _DissolveEdgeRange        ("Dissolve Edge Width",      Float)         = 0
        _DissolveEdgeRangeCoord   ("Dissolve Edge Width CD",   Float)         = 0
        [HDR] _DissolveEdgeColor  ("Dissolve Edge Color",      Color)         = (1,1,1,0)
        [KeywordEnum(Add, Multiply, Tint, Select)] _DissolveBlendType("Dissolve Blend Mode", Float) = 0
        [Toggle(_DISSOLVEMASKON_ON)] _DissolveMaskOn("DissolveMask On", Float) = 0
        _DissolveMask             ("DissolveMask",             2D)            = "black" {}
        _DissolveMaskChannel      ("DissolveMask Channel",     Float)         = 1
        _DissolveMaskIntensity    ("DissolveMask Intensity",  Range(0,1))    = 1
        _DissolveMaskIntensityCoord ("DissolveMask Intensity CD", Float)    = 0
        _DissolveMaskSpeed        ("DissolveMask Speed(UV)",   Vector)        = (0,0,0,0)
        _DissMaskOffsetXCoord     ("DissMask CD Offset X",     Float)         = 0
        _DissMaskOffsetYCoord     ("DissMask CD Offset Y",     Float)         = 0
        [Enum(UV1, 0, UV2, 1, Screen, 2)] _DissMaskUVMode ("DissMask UV Mode", Float) = 0
        _DissMaskAngle         ("DissMask Angle",       Range(0,360)) = 0
        _DissMaskAngleCoord    ("DissMask Angle CD",    Float)        = 0
        _DissMaskAngleSpeed    ("DissMask Angle Speed", Float)        = 0

        [Toggle(_MASKTEXON_ON)] _MaskTexOn("=== MaskTex On ===", Float) = 0
        _MaskTex          ("Mask Tex",        2D)         = "white" {}
        _MaskChannel        ("Mask Channel",      Float)      = 1
        _MaskIntensity      ("Mask Intensity",    Range(0,1)) = 1
        _MaskIntensityCoord ("Mask Intensity CD", Float)      = 0
        _MaskSpeed        ("Mask Speed(UV)",  Vector)     = (0,0,0,0)
        _MaskOffsetXCoord ("Mask CD Offset X", Float)     = 3
        _MaskOffsetYCoord ("Mask CD Offset Y", Float)     = 4
        [Enum(UV1, 0, UV2, 1, Screen, 2)] _MaskUVMode ("Mask UV Mode", Float) = 0
        _MaskAngle             ("Mask Angle",       Range(0,360)) = 0
        _MaskAngleCoord        ("Mask Angle CD",    Float)        = 0
        _MaskAngleSpeed        ("Mask Angle Speed", Float)        = 0

        [Toggle(_NOISETEXON_ON)] _NoiseTexOn("=== NoiseTex On ===", Float) = 0
        _NoiseTex              ("Noise Tex",                    2D)         = "white" {}
        _NoiseSpeed            ("Noise Speed(UV)",              Vector)     = (0,0,0,0)
        _NoiseOffsetXCoord     ("Noise CD Offset X",            Float)      = 0
        _NoiseOffsetYCoord     ("Noise CD Offset Y",            Float)      = 0
        [Enum(UV1, 0, UV2, 1, Screen, 2)] _NoiseUVMode ("Noise UV Mode", Float) = 0
        _NoiseAngle            ("Noise Angle",       Range(0,360)) = 0
        _NoiseAngleCoord       ("Noise Angle CD",    Float)        = 0
        _NoiseAngleSpeed       ("Noise Angle Speed", Float)        = 0
        _NoiseIntensityCustom2z("Noise Intensity",               Float)      = 1
        _NoiseIntensityCoord   ("Noise Intensity CD",           Float)      = 7
        _NoiseOffset           ("Noise Offset",                 Range(0,1)) = 0.5
        _NoiseTexChannel       ("Noise Tex Channel",              Float)      = 1
        [Toggle(_NOISEFLOWMODE_FLOWMAP)] _NoiseFlowMode ("Noise Flow Mode", Float) = 0
        _NoiseFlowChannelU     ("Noise Flow Channel U",           Float)      = 0
        _NoiseFlowChannelV     ("Noise Flow Channel V",           Float)      = 1
        _NoiseDirection        ("Noise Direction",               Range(0,360)) = 0
        _NoiseTargets          ("Noise Targets",                Float)       = -1
        [Toggle(_USEUVMASK_ON)] _UseUVMask("=== Use UV Mask ===", Float) = 0
        _NoiseUVMask             ("Noise UV Mask",              2D)              = "white" {}

        _NoiseUVMaskOffsetXCoord ("NoiseUVMask CD Offset X",    Float)           = 0
        _NoiseUVMaskOffsetYCoord ("NoiseUVMask CD Offset Y",    Float)           = 0
        [Enum(UV1, 0, UV2, 1, Screen, 2)] _NoiseUVMaskUVMode ("NoiseUVMask UV Mode", Float) = 0
        _NoiseUVMaskSpeed        ("NoiseUVMask Speed(UV)",      Vector)     = (0,0,0,0)
        _NoiseUVMaskAngle      ("NoiseUVMask Angle",       Range(0,360)) = 0
        _NoiseUVMaskAngleCoord ("NoiseUVMask Angle CD",    Float)        = 0
        _NoiseUVMaskAngleSpeed ("NoiseUVMask Angle Speed", Float)        = 0
        _NoiseUVMaskChannel         ("NoiseUVMask Channel",        Float)      = 1
        _NoiseUVMaskIntensity       ("NoiseUVMask Intensity",      Range(0,1)) = 1
        _NoiseUVMaskIntensityCoord  ("NoiseUVMask Intensity CD",   Float)      = 0

        [Toggle(_VERTEXOFFSETON_ON)] _VertexOffsetOn("=== VertexOffset On ===", Float) = 0
        _VertexOffsetTex               ("VertexOffset Tex",                  2D)     = "white" {}
        _OffsetSpeed                   ("Offset Speed(UV)",                  Vector) = (0,0,0,0)
        _VtxOffsetTexXCoord            ("VtxOffsetTex CD X",                 Float)  = 0
        _VtxOffsetTexYCoord            ("VtxOffsetTex CD Y",                 Float)  = 0
        [Enum(UV1, 0, UV2, 1, Screen, 2)] _VtxOffsetUVMode ("VtxOffsetTex UV Mode", Float) = 0
        _OffsetTexAngle        ("OffsetTex Angle",       Range(0,360)) = 0
        _OffsetTexAngleCoord   ("OffsetTex Angle CD",    Float)        = 0
        _OffsetTexAngleSpeed   ("OffsetTex Angle Speed", Float)        = 0
        [KeywordEnum(Normal, World)] _OffsetDirMode ("Direction",  Float)  = 0
        _VertexOffsetBaseValue         ("Base Value",                        Range(0,1)) = 0
        _VertexOffsetTexChannel        ("VertexOffset Tex Channel",          Float)  = 1
        _VertexOffsetIntensityCustom2w ("VertexOffset Intensity",            Float)  = 0
        _VtxIntensityCoord             ("VtxIntensity CD",                   Float)  = 8
        [Toggle(_USEOFFSETMASK_ON)] _UseOffsetMask("=== Use OffsetMask ===", Float) = 0
        _VertexOffsetMaskTex           ("VertexOffset MaskTex",              2D)     = "white" {}
        _VtxOffsetMaskChannel          ("VtxOffsetMask Channel",             Float)  = 1
        _VtxOffsetMaskSpeed            ("VtxOffsetMask Speed(UV)",           Vector) = (0,0,0,0)
        _VtxOffsetMaskXCoord           ("VtxOffsetMask CD X",                Float)  = 0
        _VtxOffsetMaskYCoord           ("VtxOffsetMask CD Y",                Float)  = 0
        [Enum(UV1, 0, UV2, 1, Screen, 2)] _VtxOffsetMaskUVMode ("VtxOffsetMask UV Mode", Float) = 0
        _VtxOffsetMaskAngle      ("VtxOffsetMask Angle",       Range(0,360)) = 0
        _VtxOffsetMaskAngleCoord ("VtxOffsetMask Angle CD",    Float)        = 0
        _VtxOffsetMaskAngleSpeed ("VtxOffsetMask Angle Speed", Float)        = 0
        _VtxOffsetMaskIntensity      ("VtxOffsetMask Intensity",    Range(0,1)) = 1
        _VtxOffsetMaskIntensityCoord ("VtxOffsetMask Intensity CD", Float)      = 0
        _OffsetDirectionXYZ            ("Axis Scale(XYZ)",                   Vector) = (1,1,1,0)
        [KeywordEnum(World, Local)] _AxisScaleMode ("Axis Scale Mode", Float) = 0

        _Diff1OffsetSpeedOn      ("Diff1 Offset Speed On",     Float) = 1
        _Diff1AngleSpeedOn       ("Diff1 Angle Speed On",      Float) = 1
        _Diff2OffsetSpeedOn      ("Diff2 Offset Speed On",     Float) = 1
        _Diff2AngleSpeedOn       ("Diff2 Angle Speed On",      Float) = 1
        _DissTexOffsetSpeedOn    ("DissTex Offset Speed On",   Float) = 1
        _DissTexAngleSpeedOn     ("DissTex Angle Speed On",    Float) = 1
        _DissMaskOffsetSpeedOn   ("DissMask Offset Speed On",  Float) = 1
        _DissMaskAngleSpeedOn    ("DissMask Angle Speed On",   Float) = 1
        _MaskOffsetSpeedOn       ("Mask Offset Speed On",      Float) = 1
        _MaskAngleSpeedOn        ("Mask Angle Speed On",       Float) = 1
        _NoiseOffsetSpeedOn      ("Noise Offset Speed On",     Float) = 1
        _NoiseAngleSpeedOn       ("Noise Angle Speed On",      Float) = 1
        _NoiseUVMaskOffsetSpeedOn ("NoiseUVMask Offset Speed On",Float) = 1
        _NoiseUVMaskAngleSpeedOn  ("NoiseUVMask Angle Speed On", Float) = 1
        _VtxTexOffsetSpeedOn     ("VtxTex Offset Speed On",    Float) = 1
        _VtxTexAngleSpeedOn      ("VtxTex Angle Speed On",     Float) = 1
        _VtxMaskOffsetSpeedOn    ("VtxMask Offset Speed On",   Float) = 1
        _VtxMaskAngleSpeedOn     ("VtxMask Angle Speed On",    Float) = 1

        _Diff1WrapMode         ("Diff1 Wrap Mode",         Float) = 1
        _Diff1WrapModeU        ("Diff1 Wrap Mode U",       Float) = 1
        _Diff1WrapModeV        ("Diff1 Wrap Mode V",       Float) = 1
        _Diff2WrapMode         ("Diff2 Wrap Mode",         Float) = 1
        _Diff2WrapModeU        ("Diff2 Wrap Mode U",       Float) = 1
        _Diff2WrapModeV        ("Diff2 Wrap Mode V",       Float) = 1
        _DissTexWrapMode       ("DissTex Wrap Mode",       Float) = 1
        _DissTexWrapModeU      ("DissTex Wrap Mode U",     Float) = 1
        _DissTexWrapModeV      ("DissTex Wrap Mode V",     Float) = 1
        _DissMaskWrapMode      ("DissMask Wrap Mode",      Float) = 1
        _DissMaskWrapModeU     ("DissMask Wrap Mode U",    Float) = 1
        _DissMaskWrapModeV     ("DissMask Wrap Mode V",    Float) = 1
        _MaskWrapMode          ("Mask Wrap Mode",          Float) = 1
        _MaskWrapModeU         ("Mask Wrap Mode U",        Float) = 1
        _MaskWrapModeV         ("Mask Wrap Mode V",        Float) = 1
        _NoiseWrapMode         ("Noise Wrap Mode",         Float) = 1
        _NoiseWrapModeU        ("Noise Wrap Mode U",       Float) = 1
        _NoiseWrapModeV        ("Noise Wrap Mode V",       Float) = 1
        _NoiseUVMaskWrapMode   ("NoiseUVMask Wrap Mode",   Float) = 1
        _NoiseUVMaskWrapModeU  ("NoiseUVMask Wrap Mode U", Float) = 1
        _NoiseUVMaskWrapModeV  ("NoiseUVMask Wrap Mode V", Float) = 1
        _VtxOffsetWrapMode     ("VtxOffset Wrap Mode",     Float) = 1
        _VtxOffsetWrapModeU    ("VtxOffset Wrap Mode U",   Float) = 1
        _VtxOffsetWrapModeV    ("VtxOffset Wrap Mode V",   Float) = 1
        _VtxMaskWrapMode       ("VtxMask Wrap Mode",       Float) = 1
        _VtxMaskWrapModeU      ("VtxMask Wrap Mode U",     Float) = 1
        _VtxMaskWrapModeV      ("VtxMask Wrap Mode V",     Float) = 1
        _MatCapWrapMode        ("MatCap Wrap Mode",        Float) = 1
        _MatCapWrapModeU       ("MatCap Wrap Mode U",      Float) = 1
        _MatCapWrapModeV       ("MatCap Wrap Mode V",      Float) = 1
        _MatCapNormalWrapMode  ("MatCapNormal Wrap Mode",  Float) = 1
        _MatCapNormalWrapModeU ("MatCapNormal Wrap Mode U",Float) = 1
        _MatCapNormalWrapModeV ("MatCapNormal Wrap Mode V",Float) = 1

        // Wrap Mode master toggle: off (0) = None; use the texture Import Settings.
        _Diff1WrapOn         ("Diff1 Wrap On",        Float) = 1
        _Diff2WrapOn         ("Diff2 Wrap On",        Float) = 1
        _DissTexWrapOn       ("DissTex Wrap On",      Float) = 1
        _DissMaskWrapOn      ("DissMask Wrap On",     Float) = 1
        _MaskWrapOn          ("Mask Wrap On",         Float) = 1
        _NoiseWrapOn         ("Noise Wrap On",        Float) = 1
        _NoiseUVMaskWrapOn   ("NoiseUVMask Wrap On",  Float) = 1
        _VtxOffsetWrapOn     ("VtxOffset Wrap On",    Float) = 1
        _VtxMaskWrapOn       ("VtxMask Wrap On",      Float) = 1
        _MatCapWrapOn        ("MatCap Wrap On",       Float) = 1
        _MatCapNormalWrapOn  ("MatCapNormal Wrap On", Float) = 1

        [KeywordEnum(Off, Fresnel, Matcap)] _RimMode ("Rim Mode", Float) = 0
        _PreMatCapCullMode ("", Float) = -1
        _FresnelProgress      ("Fresnel Progress",    Range(0,1)) = 0.5
        _FresnelProgressCoord ("Fresnel Progress CD", Float)      = 0
        _FresnelSharpness     ("Fresnel Sharpness",   Range(0,1)) = 0.5
        _FresnelSharpnessCoord("Fresnel Sharpness CD",Float)      = 0
        _FresnelIntensity     ("Fresnel Intensity",   Range(0,1)) = 1
        _FresnelIntensityCoord("Fresnel Intensity CD",Float)      = 0
        [HDR] _FresnelColor  ("Fresnel Color",    Color) = (1,1,1,1)
        [KeywordEnum(Add,Multiply,Tint)] _FresnelBlendMode("Fresnel Blend Mode", Float) = 0
        [Toggle(_INVERSE_ON)]  _Inverse          ("Inverse",         Float) = 0
        _UseFresnelAbs                         ("Use Fresnel Abs", Float) = 1
        [KeywordEnum(RGB,A)]   _FresnelChannel   ("Fresnel Channel", Float) = 0

        _MatCapTex               ("MatCap Tex",                2D)              = "white" {}
        _MatCapAngle             ("MatCap Angle",              Range(0,360))    = 0
        _MatCapAngleCoord        ("MatCap Angle CD",           Float)           = 0
        _MatCapAngleSpeed        ("MatCap Angle Speed",        Float)           = 0
        _MatCapAngleSpeedOn      ("MatCap Angle Speed On",     Float)           = 1
        [KeywordEnum(Multiply, Add)] _MatCapBlendMode ("MatCap Blend Mode", Float) = 0
        [KeywordEnum(RGB, Grayscale, RGBA, RGBtoA)] _MatCapColorMode ("MatCap Color Mode", Float) = 0
        _MatCapIntensity         ("MatCap Intensity",          Range(0,1))      = 1
        _MatCapIntensityCoord    ("MatCap Intensity CD",       Float)           = 0
        _MatCapEmissionScaler    ("MatCap Emission Scaler",    Range(1,50))     = 1
        _MatCapDim               ("MatCap Dim",                Range(0,1))      = 1
        _MatCapHue               ("MatCap Hue",                Range(-180,180)) = 0
        _MatCapSaturation        ("MatCap Saturation",         Range(-1,1))     = 0
        _MatCapContrast          ("MatCap Contrast",           Range(-1,1))     = 0
        [Toggle(_MATCAPNORMALON_ON)] _MatCapNormalOn ("MatCap Normal On", Float) = 0
        _MatCapNormalTex         ("MatCap Normal Tex",         2D)              = "bump" {}
        _MatCapNormalSpeed       ("MatCap Normal Speed(UV)",   Vector)          = (0,0,0,0)
        _MatCapNormalXCoord      ("MatCap Normal CD X",        Float)           = 0
        _MatCapNormalYCoord      ("MatCap Normal CD Y",        Float)           = 0
        [Enum(UV1, 0, UV2, 1, Screen, 2)] _MatCapNormalUVMode ("MatCap Normal UV Mode", Float) = 0
        _MatCapNormalIntensity   ("MatCap Normal Intensity",   Range(0,1))      = 0.3
        _MatCapNormalOffsetSpeedOn ("MatCap Normal Offset Speed On", Float)     = 1

        [Toggle(_DepthOn_On)] _DepthOn("Surface Fade", Float) = 0
        _PreAOTDepthOn      ("PreAOTDepthOn",         Float) = -1
        [Toggle(_EDGEMODE_ON)] _EdgeMode("Edge Mode", Float) = 0
        _DepthSoftness      ("Depth Softness",        Range(0.01,10)) = 1
        [KeywordEnum(Simple,Opacity Based)] _DepthMethod("Depth Method",    Float) = 0
        _DepthOpacityBlend  ("Depth Opacity Blend",   Range(0,1))    = 0.5
        [HDR] _EdgeColor    ("Edge Color",   Color)   = (1,1,1,1)
        _EdgeContrast       ("Edge Contrast",Range(0.1,20)) = 1
        [KeywordEnum(Add,Multiply,Tint)] _EdgeEffectColor("Blend Mode", Float) = 0
        [Toggle(_CAMERAFADE_ON)] _CameraFadeOn("Camera Fade", Float) = 0
        _CameraFadeNear ("Camera Fade Near",  Float) = 1
        _CameraFadeFar  ("Camera Fade Far",   Float) = 10
        _CameraFadeWidth("Camera Fade Width", Float) = 1
    }

    CustomEditor "VFXEssentialGUI"

    SubShader
    {
        Tags
        {
            "RenderPipeline"  = "UniversalPipeline"
            "RenderType"      = "Transparent"
            "Queue"           = "Transparent"
            "IgnoreProjector" = "True"
            "PreviewType"     = "Plane"
        }

        Blend SrcAlpha [_DstBlendMode]
        ColorMask RGB
        Cull      [_CullMode]
        ZWrite    [_ZWriteMode]
        ZTest     [_AlwaysOnTop]

        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM

            #pragma vertex   vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            // Particle System Mesh GPU Instancing (Enable Mesh GPU Instancing on the Renderer; applies automatically, with no material toggle required).
            #pragma instancing_options procedural:ParticleInstancingSetup

            #pragma shader_feature_local _USECUSTOMDATA_ON
            #pragma shader_feature_local _DIFFUSETEX2ON_ON
            #pragma shader_feature_local _DIFF2BLENDMODE_MULTIPLY _DIFF2BLENDMODE_ADD
            #pragma shader_feature_local _NOISETEXON_ON
            #pragma shader_feature_local _USEUVMASK_ON
            #pragma shader_feature_local _NOISEFLOWMODE_FLOWMAP
            #pragma shader_feature_local _DISSOLVEON_ON
            #pragma shader_feature_local _DISSOLVEMASKON_ON
            #pragma shader_feature_local _DISSOLVEBLENDTYPE_ADD _DISSOLVEBLENDTYPE_MULTIPLY _DISSOLVEBLENDTYPE_TINT _DISSOLVEBLENDTYPE_SELECT
            #pragma shader_feature_local _MASKTEXON_ON
            #pragma shader_feature_local _RIMMODE_OFF _RIMMODE_FRESNEL _RIMMODE_MATCAP
            #pragma shader_feature_local _FRESNELCHANNEL_RGB _FRESNELCHANNEL_A
            #pragma shader_feature_local _FRESNELBLENDMODE_ADD _FRESNELBLENDMODE_MULTIPLY _FRESNELBLENDMODE_TINT
            #pragma shader_feature_local _INVERSE_ON
            #pragma shader_feature_local _MATCAPBLENDMODE_MULTIPLY _MATCAPBLENDMODE_ADD
            #pragma shader_feature_local _MATCAPCOLORMODE_RGB _MATCAPCOLORMODE_RGBA _MATCAPCOLORMODE_GRAYSCALE _MATCAPCOLORMODE_RGBTOA
            #pragma shader_feature_local _MATCAPNORMALON_ON
            #pragma shader_feature_local _VERTEXOFFSETON_ON
            #pragma shader_feature_local _USEOFFSETMASK_ON
            #pragma shader_feature_local _OFFSETDIRMODE_NORMAL _OFFSETDIRMODE_WORLD
            #pragma shader_feature_local _AXISSCALEMODE_WORLD _AXISSCALEMODE_LOCAL
            #pragma shader_feature_local _DepthOn_On
            #pragma shader_feature_local _DEPTHMETHOD_SIMPLE _DEPTHMETHOD_OPACITY_BASED
            #pragma shader_feature_local _EDGEMODE_ON
            #pragma shader_feature_local _EDGEEFFECTCOLOR_ADD _EDGEEFFECTCOLOR_MULTIPLY _EDGEEFFECTCOLOR_TINT
            #pragma shader_feature_local _CAMERAFADE_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "VFXEssentialFunctions.hlsl"

            // ── Particle System Mesh GPU Instancing ─────────────────────────
            // With instancing, each particle's transform, color, Custom Data, and Texture Sheet Animation frame are not stored in vertex attributes.
            // They are stored in an array with one entry per particle. The struct below defines one entry;
            // its field order must match the order of these streams in the Renderer's Vertex Streams:
            //   Custom Data disabled: Color, AnimFrame                              (Unity's default layout; no custom definition needed)
            //   Custom Data enabled: Color, Custom1.xyzw, Custom2.xyzw, AnimFrame
            // The Inspector checks Vertex Streams against this order and provides a one-click fix.
            #ifdef _USECUSTOMDATA_ON
                #define UNITY_PARTICLE_INSTANCE_DATA VFXParticleInstanceData
                struct VFXParticleInstanceData
                {
                    float3x4 transform;
                    uint     color;
                    float4   custom1;
                    float4   custom2;
                    float    animFrame;
                };
            #endif
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ParticlesInstancing.hlsl"

            TEXTURE2D(_DiffuseTex1);         SAMPLER(sampler_DiffuseTex1);
            TEXTURE2D(_DiffuseTex2);         SAMPLER(sampler_DiffuseTex2);
            TEXTURE2D(_DissolveTex);         SAMPLER(sampler_DissolveTex);
            TEXTURE2D(_DissolveMask);        SAMPLER(sampler_DissolveMask);
            TEXTURE2D(_MaskTex);             SAMPLER(sampler_MaskTex);
            TEXTURE2D(_NoiseTex);            SAMPLER(sampler_NoiseTex);
            TEXTURE2D(_NoiseUVMask);         SAMPLER(sampler_NoiseUVMask);
            TEXTURE2D(_VertexOffsetTex);     SAMPLER(sampler_VertexOffsetTex);
            TEXTURE2D(_VertexOffsetMaskTex); SAMPLER(sampler_VertexOffsetMaskTex);
            TEXTURE2D(_MatCapTex);           SAMPLER(sampler_MatCapTex);
            TEXTURE2D(_MatCapNormalTex);     SAMPLER(sampler_MatCapNormalTex);
            CBUFFER_START(UnityPerMaterial)
                float4 _DiffuseTex1_ST;
                float4 _DiffuseTex2_ST;
                float4 _DissolveTex_ST;
                float4 _DissolveMask_ST;
                float4 _MaskTex_ST;
                float4 _NoiseTex_ST;
                float4 _NoiseUVMask_ST;
                float4 _VertexOffsetTex_ST;
                float4 _VertexOffsetMaskTex_ST;
                half4  _ColorRGBA;
                float  _EmissionScaler;
                float4 _DiffuseTex1Speed;
                float4 _DiffuseTex2Speed;
                float4 _DissSpeed;
                float4 _DissolveMaskSpeed;
                float  _DissolveTexChannel;
                float  _DissolveMaskChannel;
                float  _DissolveMaskIntensity;
                float  _DissolveMaskIntensityCoord;
                float  _DissolveRangeCustom2x;
                float  _DissolveRangeCoord;
                float  _SmoothIntensityCustom2y;
                float  _SmoothIntensityCoord;
                float  _DissolveEdgeRange;
                float  _DissolveEdgeRangeCoord;
                half4  _DissolveEdgeColor;
                float4 _MaskSpeed;
                float  _MaskChannel;
                float  _MaskIntensity;
                float  _MaskIntensityCoord;
                float4 _NoiseSpeed;
                float4 _NoiseUVMaskSpeed;
                float  _NoiseIntensityCustom2z;
                float  _NoiseIntensityCoord;
                float  _NoiseOffset;
                float  _NoiseTexChannel;
                float  _NoiseFlowChannelU; float  _NoiseFlowChannelV;
                float  _NoiseDirection;
                float  _NoiseTargets;

                float4 _OffsetSpeed;
                float4 _VtxOffsetMaskSpeed;
                float  _VertexOffsetTexChannel;
                float  _VertexOffsetBaseValue;
                float  _VertexOffsetIntensityCustom2w;
                float  _VtxIntensityCoord;
                float  _VtxOffsetMaskChannel;
                float  _VtxOffsetMaskIntensity;
                float  _VtxOffsetMaskIntensityCoord;
                float4 _OffsetDirectionXYZ;
                float _FresnelProgress;
                float _FresnelProgressCoord;
                float _FresnelSharpness;
                float _FresnelSharpnessCoord;
                float _FresnelIntensity;
                float _FresnelIntensityCoord;
                half4  _FresnelColor;
                float  _UseFresnelAbs;
                float4 _MatCapTex_ST;
                float4 _MatCapNormalTex_ST;
                float4 _MatCapNormalSpeed;
                float  _MatCapAngle;
                float  _MatCapAngleCoord;
                float  _MatCapAngleSpeed;
                float  _MatCapAngleSpeedOn;
                float  _MatCapIntensity;
                float  _MatCapIntensityCoord;
                float  _MatCapEmissionScaler;
                float  _MatCapDim;
                float  _MatCapHue;
                float  _MatCapSaturation;
                float  _MatCapContrast;
                float  _MatCapNormalXCoord;
                float  _MatCapNormalYCoord;
                float  _MatCapNormalUVMode;
                float  _MatCapNormalIntensity;
                float  _MatCapNormalOffsetSpeedOn;
                float  _DepthSoftness;
                float  _DepthOpacityBlend;
                half4  _EdgeColor;
                float  _EdgeContrast;
                float  _CameraFadeNear;
                float  _CameraFadeFar;
                float  _CameraFadeWidth;
                float  _Diff1OffsetXCoord;
                float  _Diff1OffsetYCoord;
                float  _Diff2OffsetXCoord;
                float  _Diff2OffsetYCoord;
                float  _DissTexOffsetXCoord;
                float  _DissTexOffsetYCoord;
                float  _DissMaskOffsetXCoord;
                float  _DissMaskOffsetYCoord;
                float  _MaskOffsetXCoord;
                float  _MaskOffsetYCoord;
                float  _NoiseOffsetXCoord;
                float  _NoiseOffsetYCoord;
                float  _NoiseUVMaskOffsetXCoord;
                float  _NoiseUVMaskOffsetYCoord;
                float  _VtxOffsetTexXCoord;
                float  _VtxOffsetTexYCoord;
                float  _VtxOffsetMaskXCoord;
                float  _VtxOffsetMaskYCoord;
                float  _Saturation;
                float  _Contrast;
                float  _Diff1UVMode;
                float  _Diff2UVMode;
                float  _Diff1ChannelMode;
                float  _Diff2ChannelMode;
                float  _Diff2EmissionScaler;
                float  _Diff2Intensity;
                float  _Diff2IntensityCoord;
                float  _DissTexUVMode;
                float  _DissMaskUVMode;
                float  _MaskUVMode;
                float  _NoiseUVMode;
                float  _NoiseUVMaskUVMode;
                float  _VtxOffsetUVMode;
                float  _VtxOffsetMaskUVMode;
                float  _Diff1Angle;          float  _Diff1AngleCoord;          float  _Diff1AngleSpeed;
                float  _Diff2Angle;          float  _Diff2AngleCoord;          float  _Diff2AngleSpeed;
                float  _DissTexAngle;        float  _DissTexAngleCoord;        float  _DissTexAngleSpeed;
                float  _DissMaskAngle;       float  _DissMaskAngleCoord;       float  _DissMaskAngleSpeed;
                float  _MaskAngle;           float  _MaskAngleCoord;           float  _MaskAngleSpeed;
                float  _NoiseAngle;          float  _NoiseAngleCoord;          float  _NoiseAngleSpeed;
                float  _NoiseUVMaskAngle;    float  _NoiseUVMaskAngleCoord;    float  _NoiseUVMaskAngleSpeed;
                float  _OffsetTexAngle;      float  _OffsetTexAngleCoord;      float  _OffsetTexAngleSpeed;
                float  _VtxOffsetMaskAngle;  float  _VtxOffsetMaskAngleCoord;  float  _VtxOffsetMaskAngleSpeed;
                float  _Diff1OffsetSpeedOn;      float  _Diff1AngleSpeedOn;
                float  _Diff2OffsetSpeedOn;      float  _Diff2AngleSpeedOn;
                float  _DissTexOffsetSpeedOn;    float  _DissTexAngleSpeedOn;
                float  _DissMaskOffsetSpeedOn;   float  _DissMaskAngleSpeedOn;
                float  _MaskOffsetSpeedOn;       float  _MaskAngleSpeedOn;
                float  _NoiseOffsetSpeedOn;      float  _NoiseAngleSpeedOn;
                float  _NoiseUVMaskOffsetSpeedOn; float  _NoiseUVMaskAngleSpeedOn;
                float  _NoiseUVMaskChannel;
                float  _NoiseUVMaskIntensity;     float  _NoiseUVMaskIntensityCoord;
                float  _VtxTexOffsetSpeedOn;     float  _VtxTexAngleSpeedOn;
                float  _VtxMaskOffsetSpeedOn;    float  _VtxMaskAngleSpeedOn;
                float  _Diff1WrapMode;         float  _Diff1WrapModeU;        float  _Diff1WrapModeV;
                float  _Diff2WrapMode;         float  _Diff2WrapModeU;        float  _Diff2WrapModeV;
                float  _DissTexWrapMode;       float  _DissTexWrapModeU;      float  _DissTexWrapModeV;
                float  _DissMaskWrapMode;      float  _DissMaskWrapModeU;     float  _DissMaskWrapModeV;
                float  _MaskWrapMode;          float  _MaskWrapModeU;         float  _MaskWrapModeV;
                float  _NoiseWrapMode;         float  _NoiseWrapModeU;        float  _NoiseWrapModeV;
                float  _NoiseUVMaskWrapMode;   float  _NoiseUVMaskWrapModeU;  float  _NoiseUVMaskWrapModeV;
                float  _VtxOffsetWrapMode;     float  _VtxOffsetWrapModeU;    float  _VtxOffsetWrapModeV;
                float  _VtxMaskWrapMode;       float  _VtxMaskWrapModeU;      float  _VtxMaskWrapModeV;
                float  _MatCapWrapMode;        float  _MatCapWrapModeU;       float  _MatCapWrapModeV;
                float  _MatCapNormalWrapMode;  float  _MatCapNormalWrapModeU; float  _MatCapNormalWrapModeV;
                float  _Diff1WrapOn;        float  _Diff2WrapOn;       float  _DissTexWrapOn;
                float  _DissMaskWrapOn;     float  _MaskWrapOn;        float  _NoiseWrapOn;
                float  _NoiseUVMaskWrapOn;  float  _VtxOffsetWrapOn;   float  _VtxMaskWrapOn;
                float  _MatCapWrapOn;       float  _MatCapNormalWrapOn;
                // Each texture's TexelSize (xy = 1/width, 1/height), used for the half-texel inset in Wrap Mode sampling.
                float4 _DiffuseTex1_TexelSize;        float4 _DiffuseTex2_TexelSize;
                float4 _DissolveTex_TexelSize;        float4 _DissolveMask_TexelSize;
                float4 _MaskTex_TexelSize;            float4 _NoiseTex_TexelSize;
                float4 _NoiseUVMask_TexelSize;        float4 _VertexOffsetTex_TexelSize;
                float4 _VertexOffsetMaskTex_TexelSize; float4 _MatCapTex_TexelSize;
                float4 _MatCapNormalTex_TexelSize;
            CBUFFER_END

            // ── Custom Data ────────────────────────────────────────────────────
            // CD(property) = the value of the Custom Data channel assigned to that property; 0 if assigned to None or Enable Custom Data is off.
            // Use "material value + CD(...)" throughout the shader: with particle Custom Data left at 0, toggling Enable Custom Data leaves the appearance unchanged.
            // Requires two float4 variables named cd1 / cd2 in scope (read from IN at the start of vert / frag; pass them as parameters to helper functions).
            #ifdef _USECUSTOMDATA_ON
                #define CD(coord) GetCoordValue(coord, cd1, cd2)
            #else
                #define CD(coord) 0.0
            #endif

            // ── Texture slots: consistent property names generate each slot's four Wrap Mode properties / sampler / TexelSize ──
            #define WRAP_AXES(prefix)             WrapAxes(prefix##WrapMode, prefix##WrapModeU, prefix##WrapModeV, prefix##WrapOn)
            #define SAMPLE_SLOT(tex, uv, wm)      SampleWrap2D(TEXTURE2D_ARGS(tex, sampler##tex), uv, wm, tex##_TexelSize)
            #define SAMPLE_SLOT_LOD0(tex, uv, wm) SampleWrap2DLod(TEXTURE2D_ARGS(tex, sampler##tex), uv, 0.0, wm, tex##_TexelSize)

            // Scene depth -> distance from the camera. Perspective and orthographic cameras require different conversions (UI / 2D cameras are usually orthographic).
            float SceneEyeDepth(float2 screenUV)
            {
                float raw = SampleSceneDepth(screenUV);
                return unity_OrthoParams.w == 0.0 ? LinearEyeDepth(raw, _ZBufferParams) : LinearDepthToEyeDepth(raw);
            }

            // Edge Mode intensity. softFactor: 0 = at the scene surface; 1 = sufficiently far from it.
            half DepthEdgeMask(float softFactor)
            {
                return (half)(pow(1.0 - softFactor, 1.0 / max(_EdgeContrast, 0.001)) * _EdgeColor.a);
            }

            // Use regular sampling in the fragment stage; the vertex stage has no screen-space derivatives, so sample the highest-resolution mip at a fixed LOD.
            #if defined(SHADER_STAGE_FRAGMENT)
                #define SAMPLE_SLOT_STAGE(tex, uv, wm) SAMPLE_SLOT(tex, uv, wm)
            #else
                #define SAMPLE_SLOT_STAGE(tex, uv, wm) SAMPLE_SLOT_LOD0(tex, uv, wm)
            #endif

            // ── Distortion: UV displacement ──────────────────────────────────────────
            // Compute once in the fragment stage and add to each texture's UVs according to Targets.
            // If Targets includes Vertex Offset / Offset Mask, also compute in the vertex stage to distort the Vertex Offset textures.
            #ifdef _NOISETEXON_ON
            float2 DistortionOffset(float2 uv1, float2 uv2, float2 screenUV, float4 cd1, float4 cd2)
            {
                int2   wNoise  = WRAP_AXES(_Noise);
                float2 noiseUV = SlotUV(SelUV(uv1, uv2, screenUV, _NoiseUVMode),
                                        _NoiseAngle + SpinDeg(_NoiseAngleSpeed * _NoiseAngleSpeedOn) + CD(_NoiseAngleCoord),
                                        _NoiseTex_ST,
                                        ScrollOffset(_NoiseSpeed.xy * _NoiseOffsetSpeedOn, wNoise)
                                        + float2(CD(_NoiseOffsetXCoord), CD(_NoiseOffsetYCoord)));
                float4 noiseSample    = SAMPLE_SLOT_STAGE(_NoiseTex, noiseUV, wNoise);
                float  noiseIntensity = _NoiseIntensityCustom2z + CD(_NoiseIntensityCoord);

                #ifdef _USEUVMASK_ON
                {
                    int2   wMask  = WRAP_AXES(_NoiseUVMask);
                    float2 maskUV = SlotUV(SelUV(uv1, uv2, screenUV, _NoiseUVMaskUVMode),
                                           _NoiseUVMaskAngle + SpinDeg(_NoiseUVMaskAngleSpeed * _NoiseUVMaskAngleSpeedOn) + CD(_NoiseUVMaskAngleCoord),
                                           _NoiseUVMask_ST,
                                           ScrollOffset(_NoiseUVMaskSpeed.xy * _NoiseUVMaskOffsetSpeedOn, wMask)
                                           + float2(CD(_NoiseUVMaskOffsetXCoord), CD(_NoiseUVMaskOffsetYCoord)));
                    float maskVal = SAMPLE_SLOT_STAGE(_NoiseUVMask, maskUV, wMask)[(int)_NoiseUVMaskChannel];
                    noiseIntensity *= lerp(1.0, maskVal, saturate(_NoiseUVMaskIntensity + CD(_NoiseUVMaskIntensityCoord)));
                }
                #endif

                #ifdef _NOISEFLOWMODE_FLOWMAP
                // Flow Map: the two channels encode displacement along U / V respectively.
                return (float2(noiseSample[(int)_NoiseFlowChannelU], noiseSample[(int)_NoiseFlowChannelV]) * 2.0 - 1.0) * noiseIntensity;
                #else
                // Standard Distortion: single-channel grayscale drives displacement along Direction (_NoiseDirection).
                float dirRad = radians(_NoiseDirection);
                return (noiseSample[(int)_NoiseTexChannel] - _NoiseOffset) * noiseIntensity * float2(-sin(dirRad), -cos(dirRad));
                #endif
            }
            #endif

            struct Attributes
            {
                float4 positionOS  : POSITION;
                half4  color       : COLOR;
                // .xy = UV1; .zw = UV2 (the second UV set in particle Custom Vertex Streams)
                float4 uv0         : TEXCOORD0;
                // Particles: Custom1; MeshRenderer: .xy = the mesh's UV2 (TEXCOORD1 is reused).
                float4 customData1 : TEXCOORD1;
                float4 customData2 : TEXCOORD2;
                float3 normalOS    : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS  : SV_POSITION;
                half4  color       : COLOR;
                float4 uv0         : TEXCOORD0; // .xy = UV1; .zw = UV2
                float4 customData1 : TEXCOORD1;
                float4 customData2 : TEXCOORD2;
                float4 screenPos   : TEXCOORD3;
                float3 normalWS    : TEXCOORD4;
                float3 positionWS  : TEXCOORD5;
                float  eyeDepth    : TEXCOORD6;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);

                // ── Per-vertex / per-particle inputs ──────────────────────────────────
                half4  vertexColor = IN.color;
                float4 cd1 = IN.customData1;
                float4 cd2 = IN.customData2;
                float2 uv1 = IN.uv0.xy;
                // Particles (Custom Data ON): TEXCOORD0.zw = the second UV set in Custom Vertex Streams.
                // MeshRenderer (Custom Data OFF): TEXCOORD1.xy (= customData1.xy) = the mesh's UV2.
                #ifdef _USECUSTOMDATA_ON
                float2 uv2 = IN.uv0.zw;
                #else
                float2 uv2 = IN.customData1.xy;
                #endif

                // Particle Mesh GPU Instancing renders the mesh's own vertex data;
                // read particle color / Custom Data / Texture Sheet Animation frame from the array with one entry per particle instead.
                #if defined(UNITY_PARTICLE_INSTANCING_ENABLED)
                {
                    UNITY_PARTICLE_INSTANCE_DATA inst = unity_ParticleInstanceData[unity_InstanceID];
                    // Multiply the mesh's vertex colors, if used, by particle color (matching Unity's behavior without instancing).
                    vertexColor = lerp(half4(1, 1, 1, 1), vertexColor, unity_ParticleUseMeshColors)
                                * (half4)UnpackFromR8G8B8A8(inst.color);
                    #ifdef _USECUSTOMDATA_ON
                    cd1 = inst.custom1;
                    cd2 = inst.custom2;
                    #endif
                    // The mesh's second UV set is in TEXCOORD1.
                    uv2 = IN.customData1.xy;
                    // Texture Sheet Animation: Unity adjusts UVs without instancing; with instancing, compute the frame's atlas position from its frame number.
                    if (unity_ParticleUVShiftData.x != 0.0)
                    {
                        float  tilesX    = unity_ParticleUVShiftData.y;
                        float2 tileScale = unity_ParticleUVShiftData.zw;
                        float  frame     = floor(inst.animFrame);
                        float  row       = floor(frame / tilesX);
                        float  col       = frame - row * tilesX;
                        uv1 = uv1 * tileScale + float2(col * tileScale.x, (1.0 - tileScale.y) - row * tileScale.y);
                    }
                }
                #endif

                // ── Vertex Offset ──────────────────────────────────────────────
                #ifdef _VERTEXOFFSETON_ON
                {
                    float4 sp       = ComputeScreenPos(TransformObjectToHClip(IN.positionOS.xyz));
                    float2 screenUV = sp.xy / sp.w;

                    // Distortion of Vertex Offset textures: sample in the vertex stage only if selected in Targets (32 = Vertex Offset, 64 = Offset Mask).
                    float2 vtxNoise        = 0;
                    int    vtxNoiseTargets = 0;
                    #ifdef _NOISETEXON_ON
                    vtxNoiseTargets = (int)_NoiseTargets;
                    if ((vtxNoiseTargets & 96) != 0)
                        vtxNoise = DistortionOffset(uv1, uv2, screenUV, cd1, cd2);
                    #endif

                    int2   wOffset  = WRAP_AXES(_VtxOffset);
                    float2 offsetUV = SlotUV(SelUV(uv1, uv2, screenUV, _VtxOffsetUVMode) + NoiseIf(vtxNoise, vtxNoiseTargets, 32),
                                             _OffsetTexAngle + SpinDeg(_OffsetTexAngleSpeed * _VtxTexAngleSpeedOn) + CD(_OffsetTexAngleCoord),
                                             _VertexOffsetTex_ST,
                                             ScrollOffset(_OffsetSpeed.xy * _VtxTexOffsetSpeedOn, wOffset)
                                             + float2(CD(_VtxOffsetTexXCoord), CD(_VtxOffsetTexYCoord)));
                    float offsetScalar = (SAMPLE_SLOT_LOD0(_VertexOffsetTex, offsetUV, wOffset)[(int)_VertexOffsetTexChannel] - _VertexOffsetBaseValue)
                                       * (_VertexOffsetIntensityCustom2w + CD(_VtxIntensityCoord));

                    #ifdef _USEOFFSETMASK_ON
                    {
                        int2   wMask  = WRAP_AXES(_VtxMask);
                        float2 maskUV = SlotUV(SelUV(uv1, uv2, screenUV, _VtxOffsetMaskUVMode) + NoiseIf(vtxNoise, vtxNoiseTargets, 64),
                                               _VtxOffsetMaskAngle + SpinDeg(_VtxOffsetMaskAngleSpeed * _VtxMaskAngleSpeedOn) + CD(_VtxOffsetMaskAngleCoord),
                                               _VertexOffsetMaskTex_ST,
                                               ScrollOffset(_VtxOffsetMaskSpeed.xy * _VtxMaskOffsetSpeedOn, wMask)
                                               + float2(CD(_VtxOffsetMaskXCoord), CD(_VtxOffsetMaskYCoord)));
                        float maskVal = SAMPLE_SLOT_LOD0(_VertexOffsetMaskTex, maskUV, wMask)[(int)_VtxOffsetMaskChannel];
                        offsetScalar *= lerp(1.0, maskVal, saturate(_VtxOffsetMaskIntensity + CD(_VtxOffsetMaskIntensityCoord)));
                    }
                    #endif

                    float3 offsetDirOS;
                    #if defined(_OFFSETDIRMODE_WORLD)
                        // World: Axis Scale specifies the offset direction in world space.
                        offsetDirOS = TransformWorldToObjectDir(_OffsetDirectionXYZ.xyz, false);
                    #else
                        // Normal: offset along the vertex normal. Axis Scale scales this direction per axis;
                        // Scale Mode selects the axes: World = world axes, Local = object axes.
                        #if defined(_AXISSCALEMODE_WORLD)
                        offsetDirOS = TransformWorldToObjectDir(TransformObjectToWorldDir(IN.normalOS) * _OffsetDirectionXYZ.xyz, false);
                        #else
                        offsetDirOS = IN.normalOS * _OffsetDirectionXYZ.xyz;
                        #endif
                    #endif
                    IN.positionOS.xyz += offsetScalar * offsetDirOS;
                }
                #endif

                float3 positionWS  = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionCS     = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv0            = float4(uv1, uv2);
                OUT.color          = vertexColor;
                OUT.customData1    = cd1;
                OUT.customData2    = cd2;
                OUT.screenPos      = ComputeScreenPos(OUT.positionCS);
                OUT.normalWS       = TransformObjectToWorldNormal(IN.normalOS);
                OUT.positionWS     = positionWS;
                OUT.eyeDepth       = -TransformWorldToView(positionWS).z;

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);

                float2 uv1 = IN.uv0.xy;
                float2 uv2 = IN.uv0.zw;   // The vertex stage has already selected the source of the second UV set (particles / meshes / instancing).
                float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
                float4 cd1 = IN.customData1;
                float4 cd2 = IN.customData2;

                // ── Distortion (UV displacement, added to each texture's UVs below according to Targets)──────────
                float2 noise        = 0;
                int    noiseTargets = 0;
                #ifdef _NOISETEXON_ON
                noiseTargets = (int)_NoiseTargets;
                noise        = DistortionOffset(uv1, uv2, screenUV, cd1, cd2);
                #endif

                // ── Diffuse 1 ──────────────────────────────────────────────────
                int2   wDiff1  = WRAP_AXES(_Diff1);
                float2 diff1UV = SlotUV(SelUV(uv1, uv2, screenUV, _Diff1UVMode) + NoiseIf(noise, noiseTargets, 1),
                                        _Diff1Angle + SpinDeg(_Diff1AngleSpeed * _Diff1AngleSpeedOn) + CD(_Diff1AngleCoord),
                                        _DiffuseTex1_ST,
                                        ScrollOffset(_DiffuseTex1Speed.xy * _Diff1OffsetSpeedOn, wDiff1)
                                        + float2(CD(_Diff1OffsetXCoord), CD(_Diff1OffsetYCoord)));
                // Texture Channel determines whether this texture layer provides color, Alpha, or both.
                // An unused output takes the blend operation's neutral value: 1 for Multiply, 0 for Add.
                // Initialize it to 1 (Diffuse 1 alone, or Diffuse 2 in Multiply mode); the Diffuse 2 Add path below changes it to 0.
                half3 c1; half a1, hasColor1, hasAlpha1;
                DiffuseChannel((half4)SAMPLE_SLOT(_DiffuseTex1, diff1UV, wDiff1), _Diff1ChannelMode, c1, a1, hasColor1, hasAlpha1);
                half4 mainColor = half4(lerp((half3)1, c1, hasColor1), lerp(1.0h, a1, hasAlpha1));

                // ── Diffuse 2 ──────────────────────────────────────────────────
                #ifdef _DIFFUSETEX2ON_ON
                {
                    int2   wDiff2  = WRAP_AXES(_Diff2);
                    float2 diff2UV = SlotUV(SelUV(uv1, uv2, screenUV, _Diff2UVMode) + NoiseIf(noise, noiseTargets, 2),
                                            _Diff2Angle + SpinDeg(_Diff2AngleSpeed * _Diff2AngleSpeedOn) + CD(_Diff2AngleCoord),
                                            _DiffuseTex2_ST,
                                            ScrollOffset(_DiffuseTex2Speed.xy * _Diff2OffsetSpeedOn, wDiff2)
                                            + float2(CD(_Diff2OffsetXCoord), CD(_Diff2OffsetYCoord)));
                    half3 c2; half a2, hasColor2, hasAlpha2;
                    DiffuseChannel((half4)SAMPLE_SLOT(_DiffuseTex2, diff2UV, wDiff2), _Diff2ChannelMode, c2, a2, hasColor2, hasAlpha2);
                    c2 *= (half)_Diff2EmissionScaler;   // 1 = original brightness
                    half d2i = (half)saturate(_Diff2Intensity + CD(_Diff2IntensityCoord));

                    // An output (color or Alpha) unused by the second layer does not participate in blending; the corresponding base-layer output is unchanged.
                    #if defined(_DIFF2BLENDMODE_ADD)
                        // The neutral value for Add is 0: an output unused by Diffuse 1 but provided by the second layer starts from 0
                        // (e.g. Diffuse 1 = RGB, Diffuse 2 = A -> Alpha comes entirely from the second layer).
                        // An output unused by both layers stays at 1 (white color / opaque Alpha), otherwise the result would be invisible.
                        mainColor.rgb *= 1.0h - hasColor2 * (1.0h - hasColor1);
                        mainColor.a   *= 1.0h - hasAlpha2 * (1.0h - hasAlpha1);
                        // Add color weighted by the second layer's Alpha if present (no addition in transparent regions); otherwise add color directly.
                        mainColor.rgb += c2 * lerp(1.0h, a2, hasAlpha2) * (hasColor2 * d2i);
                        // Add Alpha only if the second layer provides Alpha.
                        mainColor.a    = saturate(mainColor.a + a2 * (hasAlpha2 * d2i));
                    #else // Multiply (default)
                        // Intensity of 0 restores the multiplier to 1, leaving the base layer unchanged.
                        mainColor.rgb *= lerp((half3)1, c2, hasColor2 * d2i);
                        mainColor.a   *= lerp(1.0h, a2, hasAlpha2 * d2i);
                    #endif
                }
                #endif

                // gate = the product of all visibility-reducing factors: Dissolve, Mask, Fresnel (Output Channel A), vertex color Alpha, and Tint Color Alpha.
                // The texture's own Alpha remains in mainColor.a and is multiplied by gate only at final composition.
                // This lets effects that add Alpha (MatCap, Edge Mode) also multiply their contribution by gate,
                // so regions already faded out / dissolved / hidden by Mask cannot become visible again.
                half gate = 1;

                // ── Dissolve ───────────────────────────────────────────────────
                half dissTintEdge = 0;   // Tint mode: edge color coverage (apply the color after Tint Color).
                #ifdef _DISSOLVEON_ON
                {
                    float dissRange = _DissolveRangeCustom2x + CD(_DissolveRangeCoord);
                    float smooth    = saturate(_SmoothIntensityCustom2y + CD(_SmoothIntensityCoord));
                    float edgeRange = _DissolveEdgeRange + CD(_DissolveEdgeRangeCoord);

                    #ifdef _DISSOLVEMASKON_ON
                    {
                        int2   wDissMask  = WRAP_AXES(_DissMask);
                        float2 dissMaskUV = SlotUV(SelUV(uv1, uv2, screenUV, _DissMaskUVMode) + NoiseIf(noise, noiseTargets, 16),
                                                   _DissMaskAngle + SpinDeg(_DissMaskAngleSpeed * _DissMaskAngleSpeedOn) + CD(_DissMaskAngleCoord),
                                                   _DissolveMask_ST,
                                                   ScrollOffset(_DissolveMaskSpeed.xy * _DissMaskOffsetSpeedOn, wDissMask)
                                                   + float2(CD(_DissMaskOffsetXCoord), CD(_DissMaskOffsetYCoord)));
                        // Brighter regions of the Dissolve Mask dissolve later.
                        dissRange -= SAMPLE_SLOT(_DissolveMask, dissMaskUV, wDissMask)[(int)_DissolveMaskChannel]
                                   * (_DissolveMaskIntensity + CD(_DissolveMaskIntensityCoord));
                    }
                    #endif

                    int2   wDiss  = WRAP_AXES(_DissTex);
                    float2 dissUV = SlotUV(SelUV(uv1, uv2, screenUV, _DissTexUVMode) + NoiseIf(noise, noiseTargets, 8),
                                           _DissTexAngle + SpinDeg(_DissTexAngleSpeed * _DissTexAngleSpeedOn) + CD(_DissTexAngleCoord),
                                           _DissolveTex_ST,
                                           ScrollOffset(_DissSpeed.xy * _DissTexOffsetSpeedOn, wDiss)
                                           + float2(CD(_DissTexOffsetXCoord), CD(_DissTexOffsetYCoord)));
                    float diss = SAMPLE_SLOT(_DissolveTex, dissUV, wDiss)[(int)_DissolveTexChannel];

                    float dissAlphaF, edgeMaskF;
                    HardDissolveEdges(diss, dissRange, smooth, edgeRange, dissAlphaF, edgeMaskF);
                    half dissAlpha = (half)dissAlphaF;
                    half edgeMask  = (half)edgeMaskF;

                    #if defined(_DISSOLVEBLENDTYPE_MULTIPLY)
                    mainColor.rgb *= lerp((half3)1, _DissolveEdgeColor.rgb, edgeMask * _DissolveEdgeColor.a);
                    gate *= dissAlpha;
                    #elif defined(_DISSOLVEBLENDTYPE_TINT)
                    dissTintEdge = edgeMask / max(dissAlpha, 0.0001h);
                    gate *= dissAlpha - edgeMask * (1.0h - _DissolveEdgeColor.a);
                    #elif defined(_DISSOLVEBLENDTYPE_SELECT)
                    gate *= edgeMask;
                    #else // Add
                    mainColor.rgb += edgeMask * _DissolveEdgeColor.a * _DissolveEdgeColor.rgb;
                    gate *= dissAlpha;
                    #endif
                }
                #endif

                // ── Mask ───────────────────────────────────────────────────────
                #ifdef _MASKTEXON_ON
                {
                    int2   wMask  = WRAP_AXES(_Mask);
                    float2 maskUV = SlotUV(SelUV(uv1, uv2, screenUV, _MaskUVMode) + NoiseIf(noise, noiseTargets, 4),
                                           _MaskAngle + SpinDeg(_MaskAngleSpeed * _MaskAngleSpeedOn) + CD(_MaskAngleCoord),
                                           _MaskTex_ST,
                                           ScrollOffset(_MaskSpeed.xy * _MaskOffsetSpeedOn, wMask)
                                           + float2(CD(_MaskOffsetXCoord), CD(_MaskOffsetYCoord)));
                    half maskVal = (half)SAMPLE_SLOT(_MaskTex, maskUV, wMask)[(int)_MaskChannel];
                    gate *= lerp(1.0h, maskVal, (half)saturate(_MaskIntensity + CD(_MaskIntensityCoord)));
                }
                #endif

                // ── Scene depth (shared by Surface Fade / Edge Mode; sampled only once)────────────
                #ifdef _DepthOn_On
                float depthGap = SceneEyeDepth(screenUV) - IN.eyeDepth;   // Distance to the scene surface behind this pixel.
                #endif
                // Compute intersection edge intensity early to suppress Fresnel later (edge color takes priority).
                half depthEdge = 0;
                #if defined(_DepthOn_On) && defined(_EDGEMODE_ON)
                depthEdge = DepthEdgeMask(saturate(depthGap / max(_DepthSoftness, 0.001)));
                #endif

                // ── Rim (Fresnel / MatCap) ─────────────────────────────────────
                half mcAddAlpha  = 0;   // Alpha added by MatCap.
                half fresnelMask = 0;   // Defer Fresnel (RGB) composition until after Tint Color.
                #if defined(_RIMMODE_FRESNEL)
                {
                    float3 normalWS  = normalize(IN.normalWS);
                    float3 viewDirWS = normalize(GetWorldSpaceViewDir(IN.positionWS));
                    float  ndotv     = dot(normalWS, viewDirWS);
                    float  rim       = 1.0 - lerp(ndotv, abs(ndotv), _UseFresnelAbs);
                    #ifdef _INVERSE_ON
                    rim = 1.0 - rim;
                    #endif
                    float prog  = min(1.0, _FresnelProgress + CD(_FresnelProgressCoord));
                    // Transition width. Sharpness = 1 gives zero width and a division by zero in smoothstep, so retain a minimal width.
                    float width = max(1.0 - min(1.0, _FresnelSharpness + CD(_FresnelSharpnessCoord)), 1e-4);
                    float fresnel = smoothstep(lerp(-width, 1.0, prog), lerp(0.0, 1.0 + width, prog), rim)
                                  * (_FresnelIntensity + CD(_FresnelIntensityCoord));
                    #if defined(_FRESNELCHANNEL_A)
                    gate *= (half)fresnel;
                    #else
                    fresnelMask = (half)(fresnel * _FresnelColor.a);
                    #endif
                }
                #elif defined(_RIMMODE_MATCAP)
                {
                    float3 normalWS = normalize(IN.normalWS);
                    #ifdef _MATCAPNORMALON_ON
                    {
                        int2   wNormal  = WRAP_AXES(_MatCapNormal);
                        float2 normalBaseUV = SelUV(uv1, uv2, screenUV, _MatCapNormalUVMode) * _MatCapNormalTex_ST.xy;
                        float2 normalUV = normalBaseUV + _MatCapNormalTex_ST.zw
                                        + ScrollOffset(_MatCapNormalSpeed.xy * _MatCapNormalOffsetSpeedOn, wNormal)
                                        + float2(CD(_MatCapNormalXCoord), CD(_MatCapNormalYCoord));
                        float3 normalTS = UnpackNormal(SAMPLE_SLOT(_MatCapNormalTex, normalUV, wNormal));
                        normalTS.xy *= _MatCapNormalIntensity;
                        normalTS.z   = sqrt(max(0.0001, 1.0 - saturate(dot(normalTS.xy, normalTS.xy))));
                        // Derive tangent directions from screen-space derivatives of the normal map's actual UVs; no vertex tangents required (particles need no Tangent stream).
                        normalWS = TangentToWorldNoTangent(normalTS, normalWS, IN.positionWS, normalBaseUV);
                    }
                    #endif

                    // View-space reflection direction -> MatCap UVs (per-pixel view direction; within [0,1] before Tiling / Offset and rotation are applied).
                    float3 normalVS = mul((float3x3)UNITY_MATRIX_V, normalWS);
                    float3 reflVS   = reflect(normalize(TransformWorldToView(IN.positionWS)), normalVS);
                    reflVS.z += 1.0;
                    float2 mcUV = reflVS.xy / (2.0 * length(reflVS)) + 0.5;
                    mcUV = RotateUV(mcUV * _MatCapTex_ST.xy + _MatCapTex_ST.zw,
                                    _MatCapAngle + SpinDeg(_MatCapAngleSpeed * _MatCapAngleSpeedOn) + CD(_MatCapAngleCoord));

                    int2   wMatCap = WRAP_AXES(_MatCap);
                    float4 mc      = SAMPLE_SLOT(_MatCapTex, mcUV, wMatCap);
                    mc.rgb *= _MatCapEmissionScaler;
                    #if !defined(_MATCAPBLENDMODE_ADD)   // Multiply
                    mc.rgb *= _MatCapDim;
                    #endif

                    // Hue (rotate color around the (1,1,1) axis).
                    if (_MatCapHue != 0) {
                        float hueRad = radians(_MatCapHue);
                        float hc = cos(hueRad), hs = sin(hueRad);
                        float3 k = float3(0.57735, 0.57735, 0.57735);
                        mc.rgb = mc.rgb * hc + cross(k, mc.rgb) * hs + k * dot(k, mc.rgb) * (1.0 - hc);
                    }
                    // Saturation (0 = unchanged, -1 = grayscale, +1 = double chroma).
                    float mcLum = dot(mc.rgb, float3(0.299, 0.587, 0.114));
                    mc.rgb = lerp(float3(mcLum, mcLum, mcLum), mc.rgb, 1.0 + _MatCapSaturation);
                    // Contrast (0 = unchanged).
                    mc.rgb = max(0.0, (mc.rgb - 0.5) * (1.0 + _MatCapContrast) + 0.5);

                    half mcInt  = (half)saturate(_MatCapIntensity + CD(_MatCapIntensityCoord));
                    half mcGray = (half)dot(mc.rgb, float3(0.299, 0.587, 0.114));
                    half mcA    = (half)mc.a;

                    #if defined(_MATCAPCOLORMODE_RGBTOA)
                        // RGBtoA: MatCap grayscale drives only Alpha, leaving RGB unchanged.
                        #if defined(_MATCAPBLENDMODE_ADD)
                        mcAddAlpha = mcGray * mcInt;
                        #else // Multiply
                        mainColor.a *= lerp(1.0h, mcGray, mcInt);
                        #endif
                    #else
                        #if defined(_MATCAPCOLORMODE_GRAYSCALE)
                        half3 mcColor = half3(mcGray, mcGray, mcGray);
                        #else
                        half3 mcColor = (half3)mc.rgb;
                        #endif

                        #if defined(_MATCAPBLENDMODE_ADD)
                            mainColor.rgb += mcColor * mcInt;
                            #if defined(_MATCAPCOLORMODE_RGBA)
                            mcAddAlpha = mcA * mcInt;
                            #endif
                        #else // Multiply
                            mainColor.rgb *= lerp((half3)1, mcColor, mcInt);
                            #if defined(_MATCAPCOLORMODE_RGBA)
                            mainColor.a *= lerp(1.0h, mcA, mcInt);
                            #endif
                        #endif
                    #endif
                }
                #endif

                // ── Final color composition ───────────────────────────────────────────────────
                // Emission Scaler: brightness multiplier. 1 = original brightness; higher values boost HDR brightness.
                half4 finalColor;
                finalColor.rgb = mainColor.rgb * IN.color.rgb * _ColorRGBA.rgb * (half)_EmissionScaler;
                gate          *= IN.color.a * _ColorRGBA.a;
                finalColor.a   = saturate(mainColor.a + mcAddAlpha) * gate;

                #if defined(_DISSOLVEON_ON) && defined(_DISSOLVEBLENDTYPE_TINT)
                finalColor.rgb = lerp(finalColor.rgb, _DissolveEdgeColor.rgb, dissTintEdge);
                #endif

                // ── Fresnel (deferred composition: after Dissolve Tint, before Depth)
                #if defined(_RIMMODE_FRESNEL) && !defined(_FRESNELCHANNEL_A)
                {
                    half frMask = fresnelMask * (1.0h - depthEdge);
                    #if defined(_FRESNELBLENDMODE_MULTIPLY)
                    finalColor.rgb *= lerp((half3)1, _FresnelColor.rgb, frMask);
                    #elif defined(_FRESNELBLENDMODE_TINT)
                    finalColor.rgb = lerp(finalColor.rgb, _FresnelColor.rgb, frMask);
                    #else // Add
                    finalColor.rgb += _FresnelColor.rgb * frMask;
                    #endif
                }
                #endif

                // ── Depth: Surface Fade, or Edge Mode Add / Multiply (Tint follows color adjustments)
                #if defined(_DepthOn_On) && !(defined(_EDGEMODE_ON) && defined(_EDGEEFFECTCOLOR_TINT))
                {
                    #if defined(_DEPTHMETHOD_OPACITY_BASED)
                    // More transparent pixels use a longer fade distance.
                    float softness = lerp(_DepthSoftness * lerp(1.0, 8.0, _DepthOpacityBlend), _DepthSoftness, saturate((float)finalColor.a));
                    #else
                    float softness = _DepthSoftness;
                    #endif
                    float softFactor = saturate(depthGap / max(softness, 0.001));

                    #ifndef _EDGEMODE_ON
                        finalColor.a *= (half)softFactor;
                    #else
                        half edgeA = DepthEdgeMask(softFactor);
                        #if defined(_EDGEEFFECTCOLOR_MULTIPLY)
                        finalColor.rgb *= lerp((half3)1, (half3)_EdgeColor.rgb, edgeA);
                        #else // Add
                        finalColor.rgb += (half3)_EdgeColor.rgb * edgeA;
                        #endif
                        // Edges remain visible in transparent texture regions but are still constrained by gate (faded particles leave no edge lines).
                        finalColor.a = max(finalColor.a, edgeA * gate);
                    #endif
                }
                #endif

                // ── Saturation (cheap luminance lerp; default 0 = unchanged, -1 = grayscale, +1 = double chroma)
                float lum = dot((float3)finalColor.rgb, float3(0.2126, 0.7152, 0.0722));
                finalColor.rgb = lerp((half3)lum, finalColor.rgb, (half)(1.0 + _Saturation));

                // ── Contrast (RGB: linear around a pivot of 0.5. Alpha: the same when positive; scaled toward 0 when negative, so lowering Contrast never reveals transparent areas)
                float3 cRGB = max(0.0, ((float3)finalColor.rgb - 0.5) * (1.0 + _Contrast) + 0.5);
                float  cA   = _Contrast >= 0.0
                    ? saturate(((float)finalColor.a - 0.5) * (1.0 + _Contrast) + 0.5)
                    : saturate((float)finalColor.a * (1.0 + _Contrast));
                finalColor = half4((half3)cRGB, (half)cA);

                // ── Edge Mode Tint (after color adjustments, so Saturation / Contrast do not affect the edge color)
                #if defined(_DepthOn_On) && defined(_EDGEMODE_ON) && defined(_EDGEEFFECTCOLOR_TINT)
                finalColor.rgb = lerp(finalColor.rgb, (half3)_EdgeColor.rgb, depthEdge);
                finalColor.a   = max(finalColor.a, depthEdge * gate);
                #endif

                // ── Camera Fade (fade by camera distance; applied last so edge colors fade too)
                #ifdef _CAMERAFADE_ON
                finalColor.a *= (half)saturate(min(IN.eyeDepth - _CameraFadeNear, _CameraFadeFar - IN.eyeDepth) / max(_CameraFadeWidth, 0.001));
                #endif

                return finalColor;
            }

            ENDHLSL
        }

        // ── Scene view selection outline ────────────────────────────────────────
        // Unity draws the orange selection outline from a pass tagged SceneSelectionPass. Without one it falls back to a
        // shader that has no per-particle transform, so the outline lands on the Particle System's origin (or on a single
        // mesh instance) instead of on the particles. This pass repeats the instancing setup of the forward pass so the
        // outline follows every particle. Vertex Offset is not applied here, matching URP's own particle shaders.
        Pass
        {
            Name "SceneSelectionPass"
            Tags { "LightMode" = "SceneSelectionPass" }

            Blend One Zero
            ZWrite On
            ZTest LEqual
            Cull Off
            ColorMask RGBA

            HLSLPROGRAM

            #pragma vertex   vertEditor
            #pragma fragment fragSelection
            #pragma multi_compile_instancing
            #pragma instancing_options procedural:ParticleInstancingSetup
            #pragma shader_feature_local _USECUSTOMDATA_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Same instance layout as the forward pass; see the comment there.
            #ifdef _USECUSTOMDATA_ON
                #define UNITY_PARTICLE_INSTANCE_DATA VFXParticleInstanceData
                struct VFXParticleInstanceData
                {
                    float3x4 transform;
                    uint     color;
                    float4   custom1;
                    float4   custom2;
                    float    animFrame;
                };
            #endif
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ParticlesInstancing.hlsl"

            float _ObjectId;
            float _PassValue;

            struct EditorAttributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct EditorVaryings
            {
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            EditorVaryings vertEditor(EditorAttributes IN)
            {
                EditorVaryings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            float4 fragSelection(EditorVaryings IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                return float4(_ObjectId, _PassValue, 1.0, 1.0);
            }

            ENDHLSL
        }

        // ── Scene picking (click selection in the Scene view) ───────────────────
        Pass
        {
            Name "ScenePickingPass"
            Tags { "LightMode" = "Picking" }

            Blend One Zero
            ZWrite On
            ZTest LEqual
            Cull Off
            ColorMask RGBA

            HLSLPROGRAM

            #pragma vertex   vertEditor
            #pragma fragment fragPicking
            #pragma multi_compile_instancing
            #pragma instancing_options procedural:ParticleInstancingSetup
            #pragma shader_feature_local _USECUSTOMDATA_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #ifdef _USECUSTOMDATA_ON
                #define UNITY_PARTICLE_INSTANCE_DATA VFXParticleInstanceData
                struct VFXParticleInstanceData
                {
                    float3x4 transform;
                    uint     color;
                    float4   custom1;
                    float4   custom2;
                    float    animFrame;
                };
            #endif
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ParticlesInstancing.hlsl"

            float4 _SelectionID;

            struct EditorAttributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct EditorVaryings
            {
                float4 positionCS : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            EditorVaryings vertEditor(EditorAttributes IN)
            {
                EditorVaryings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            float4 fragPicking(EditorVaryings IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                return _SelectionID;
            }

            ENDHLSL
        }
    }
}
