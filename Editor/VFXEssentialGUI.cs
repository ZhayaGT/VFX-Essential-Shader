#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[System.Flags]
enum NoiseTarget
{
    None         = 0,
    Diffuse1     = 1 << 0,
    Diffuse2     = 1 << 1,
    Mask         = 1 << 2,
    Dissolve     = 1 << 3,
    DissolveMask = 1 << 4,
    VertexOffset = 1 << 5,
    OffsetMask   = 1 << 6,
    All          = ~0,
}

public class VFXEssentialGUI : ShaderGUI
{
    static bool _RenderFoldout = true;
    static bool _Diff1Foldout = true;
    static bool _Diff2Foldout = true;
    static bool _DissolveFoldout = true;
    static bool _MaskFoldout = true;
    static bool _NoiseFoldout = true;
    static bool _VertexFoldout = true;
    static bool _RimFoldout = true;
    static bool _DepthFoldout = true;

    MaterialEditor m_Editor;
    List<string>[] _cdUsers;

    // Render Options
    MaterialProperty p_RenderPriority;
    MaterialProperty p_AlwaysOnTop;
    MaterialProperty p_CullMode;
    MaterialProperty p_DstBlendMode;
    MaterialProperty p_ZWriteMode;
    MaterialProperty p_UseCustomData;
    MaterialProperty p_Saturation;
    MaterialProperty p_Contrast;

    // Main Tex 1
    MaterialProperty p_DiffuseTex1;
    MaterialProperty p_EmissionScaler;
    MaterialProperty p_ColorRGBA;
    MaterialProperty p_DiffuseTex1Speed;
    MaterialProperty p_Diff1OffsetXCoord;
    MaterialProperty p_Diff1OffsetYCoord;
    MaterialProperty p_Diff1UVMode;
    MaterialProperty p_Diff1ChannelMode;
    MaterialProperty p_Diff1Angle; MaterialProperty p_Diff1AngleCoord; MaterialProperty p_Diff1AngleSpeed;

    // Main Tex 2
    MaterialProperty p_DiffuseTex2On;
    MaterialProperty p_Diff2BlendMode;
    MaterialProperty p_DiffuseTex2;
    MaterialProperty p_DiffuseTex2Speed;
    MaterialProperty p_Diff2OffsetXCoord;
    MaterialProperty p_Diff2OffsetYCoord;
    MaterialProperty p_Diff2UVMode;
    MaterialProperty p_Diff2ChannelMode;
    MaterialProperty p_Diff2Angle; MaterialProperty p_Diff2AngleCoord; MaterialProperty p_Diff2AngleSpeed;
    MaterialProperty p_Diff2EmissionScaler;
    MaterialProperty p_Diff2Intensity;
    MaterialProperty p_Diff2IntensityCoord;

    // Dissolve
    MaterialProperty p_DissolveOn;
    MaterialProperty p_DissolveTex;
    MaterialProperty p_DissSpeed;
    MaterialProperty p_DissTexOffsetXCoord;
    MaterialProperty p_DissTexOffsetYCoord;
    MaterialProperty p_DissTexUVMode;
    MaterialProperty p_DissTexAngle; MaterialProperty p_DissTexAngleCoord; MaterialProperty p_DissTexAngleSpeed;
    MaterialProperty p_DissolveTexChannel;
    MaterialProperty p_DissolveRangeCustom2x;
    MaterialProperty p_DissolveRangeCoord;
    MaterialProperty p_SmoothIntensityCustom2y;
    MaterialProperty p_SmoothIntensityCoord;
    MaterialProperty p_DissolveEdgeRange;
    MaterialProperty p_DissolveEdgeRangeCoord;
    MaterialProperty p_DissolveEdgeColor;
    MaterialProperty p_DissolveBlendType;
    MaterialProperty p_DissolveMaskOn;
    MaterialProperty p_DissolveMask;
    MaterialProperty p_DissolveMaskChannel;
    MaterialProperty p_DissolveMaskIntensity;
    MaterialProperty p_DissolveMaskIntensityCoord;
    MaterialProperty p_DissolveMaskSpeed;
    MaterialProperty p_DissMaskOffsetXCoord;
    MaterialProperty p_DissMaskOffsetYCoord;
    MaterialProperty p_DissMaskUVMode;
    MaterialProperty p_DissMaskAngle; MaterialProperty p_DissMaskAngleCoord; MaterialProperty p_DissMaskAngleSpeed;

    // Mask
    MaterialProperty p_MaskTexOn;
    MaterialProperty p_MaskTex;
    MaterialProperty p_MaskChannel;
    MaterialProperty p_MaskIntensity;
    MaterialProperty p_MaskIntensityCoord;
    MaterialProperty p_MaskSpeed;
    MaterialProperty p_MaskOffsetXCoord;
    MaterialProperty p_MaskOffsetYCoord;
    MaterialProperty p_MaskUVMode;
    MaterialProperty p_MaskAngle; MaterialProperty p_MaskAngleCoord; MaterialProperty p_MaskAngleSpeed;

    // Noise
    MaterialProperty p_NoiseTexOn;
    MaterialProperty p_NoiseTex;
    MaterialProperty p_NoiseSpeed;
    MaterialProperty p_NoiseOffsetXCoord;
    MaterialProperty p_NoiseOffsetYCoord;
    MaterialProperty p_NoiseUVMode;
    MaterialProperty p_NoiseAngle; MaterialProperty p_NoiseAngleCoord; MaterialProperty p_NoiseAngleSpeed;
    MaterialProperty p_NoiseTexChannel;
    MaterialProperty p_NoiseIntensityCustom2z;
    MaterialProperty p_NoiseIntensityCoord;
    MaterialProperty p_NoiseOffset;
    MaterialProperty p_NoiseDirection;
    MaterialProperty p_NoiseTargets;
    MaterialProperty p_UseUVMask;
    MaterialProperty p_NoiseUVMask;

    MaterialProperty p_NoiseUVMaskOffsetXCoord;
    MaterialProperty p_NoiseUVMaskOffsetYCoord;
    MaterialProperty p_NoiseUVMaskUVMode;
    MaterialProperty p_NoiseUVMaskSpeed;
    MaterialProperty p_NoiseUVMaskAngle; MaterialProperty p_NoiseUVMaskAngleCoord; MaterialProperty p_NoiseUVMaskAngleSpeed;
    MaterialProperty p_NoiseUVMaskChannel;
    MaterialProperty p_NoiseUVMaskIntensity; MaterialProperty p_NoiseUVMaskIntensityCoord;
    MaterialProperty p_NoiseFlowMode;
    MaterialProperty p_NoiseFlowChannelU; MaterialProperty p_NoiseFlowChannelV;

    // Vertex Offset
    MaterialProperty p_VertexOffsetOn;
    MaterialProperty p_OffsetDirMode;
    MaterialProperty p_VertexOffsetBaseValue;
    MaterialProperty p_VertexOffsetTex;
    MaterialProperty p_VertexOffsetTexChannel;
    MaterialProperty p_OffsetSpeed;
    MaterialProperty p_VtxOffsetTexXCoord;
    MaterialProperty p_VtxOffsetTexYCoord;
    MaterialProperty p_VtxOffsetUVMode;
    MaterialProperty p_OffsetTexAngle; MaterialProperty p_OffsetTexAngleCoord; MaterialProperty p_OffsetTexAngleSpeed;
    MaterialProperty p_VertexOffsetIntensityCustom2w;
    MaterialProperty p_VtxIntensityCoord;
    MaterialProperty p_UseOffsetMask;
    MaterialProperty p_VertexOffsetMaskTex;
    MaterialProperty p_VtxOffsetMaskChannel;
    MaterialProperty p_VtxOffsetMaskSpeed;
    MaterialProperty p_VtxOffsetMaskXCoord;
    MaterialProperty p_VtxOffsetMaskYCoord;
    MaterialProperty p_VtxOffsetMaskUVMode;
    MaterialProperty p_VtxOffsetMaskAngle; MaterialProperty p_VtxOffsetMaskAngleCoord; MaterialProperty p_VtxOffsetMaskAngleSpeed;
    MaterialProperty p_VtxOffsetMaskIntensity;
    MaterialProperty p_VtxOffsetMaskIntensityCoord;
    MaterialProperty p_OffsetDirectionXYZ;
    MaterialProperty p_AxisScaleMode;

    // Wrap Mode
    MaterialProperty p_Diff1WrapMode;        MaterialProperty p_Diff1WrapModeU;        MaterialProperty p_Diff1WrapModeV;
    MaterialProperty p_Diff2WrapMode;        MaterialProperty p_Diff2WrapModeU;        MaterialProperty p_Diff2WrapModeV;
    MaterialProperty p_DissTexWrapMode;      MaterialProperty p_DissTexWrapModeU;      MaterialProperty p_DissTexWrapModeV;
    MaterialProperty p_DissMaskWrapMode;     MaterialProperty p_DissMaskWrapModeU;     MaterialProperty p_DissMaskWrapModeV;
    MaterialProperty p_MaskWrapMode;         MaterialProperty p_MaskWrapModeU;         MaterialProperty p_MaskWrapModeV;
    MaterialProperty p_NoiseWrapMode;        MaterialProperty p_NoiseWrapModeU;        MaterialProperty p_NoiseWrapModeV;
    MaterialProperty p_NoiseUVMaskWrapMode;  MaterialProperty p_NoiseUVMaskWrapModeU;  MaterialProperty p_NoiseUVMaskWrapModeV;
    MaterialProperty p_VtxOffsetWrapMode;    MaterialProperty p_VtxOffsetWrapModeU;    MaterialProperty p_VtxOffsetWrapModeV;
    MaterialProperty p_VtxMaskWrapMode;      MaterialProperty p_VtxMaskWrapModeU;      MaterialProperty p_VtxMaskWrapModeV;
    MaterialProperty p_MatCapWrapMode;       MaterialProperty p_MatCapWrapModeU;       MaterialProperty p_MatCapWrapModeV;
    MaterialProperty p_MatCapNormalWrapMode; MaterialProperty p_MatCapNormalWrapModeU; MaterialProperty p_MatCapNormalWrapModeV;
    // Wrap Mode master toggle (off = None; use the texture Import Settings).
    MaterialProperty p_Diff1WrapOn;       MaterialProperty p_Diff2WrapOn;     MaterialProperty p_DissTexWrapOn;
    MaterialProperty p_DissMaskWrapOn;    MaterialProperty p_MaskWrapOn;      MaterialProperty p_NoiseWrapOn;
    MaterialProperty p_NoiseUVMaskWrapOn; MaterialProperty p_VtxOffsetWrapOn; MaterialProperty p_VtxMaskWrapOn;
    MaterialProperty p_MatCapWrapOn;      MaterialProperty p_MatCapNormalWrapOn;

    // Speed On flags
    MaterialProperty p_Diff1OffsetSpeedOn;      MaterialProperty p_Diff1AngleSpeedOn;
    MaterialProperty p_Diff2OffsetSpeedOn;      MaterialProperty p_Diff2AngleSpeedOn;
    MaterialProperty p_DissTexOffsetSpeedOn;    MaterialProperty p_DissTexAngleSpeedOn;
    MaterialProperty p_DissMaskOffsetSpeedOn;   MaterialProperty p_DissMaskAngleSpeedOn;
    MaterialProperty p_MaskOffsetSpeedOn;       MaterialProperty p_MaskAngleSpeedOn;
    MaterialProperty p_NoiseOffsetSpeedOn;      MaterialProperty p_NoiseAngleSpeedOn;
    MaterialProperty p_NoiseUVMaskOffsetSpeedOn;  MaterialProperty p_NoiseUVMaskAngleSpeedOn;
    MaterialProperty p_VtxTexOffsetSpeedOn;     MaterialProperty p_VtxTexAngleSpeedOn;
    MaterialProperty p_VtxMaskOffsetSpeedOn;    MaterialProperty p_VtxMaskAngleSpeedOn;

    // Rim (Fresnel / MatCap)
    MaterialProperty p_RimMode;
    MaterialProperty p_PreMatCapCullMode;
    MaterialProperty p_FresnelProgress;     MaterialProperty p_FresnelProgressCoord;
    MaterialProperty p_FresnelSharpness;    MaterialProperty p_FresnelSharpnessCoord;
    MaterialProperty p_FresnelIntensity;    MaterialProperty p_FresnelIntensityCoord;
    MaterialProperty p_FresnelColor;
    MaterialProperty p_FresnelBlendMode;
    MaterialProperty p_Inverse;
    MaterialProperty p_UseFresnelAbs;
    MaterialProperty p_FresnelChannel;
    MaterialProperty p_MatCapTex;
    MaterialProperty p_MatCapAngle;      MaterialProperty p_MatCapAngleCoord; MaterialProperty p_MatCapAngleSpeed;
    MaterialProperty p_MatCapAngleSpeedOn;
    MaterialProperty p_MatCapBlendMode;
    MaterialProperty p_MatCapColorMode;
    MaterialProperty p_MatCapIntensity;  MaterialProperty p_MatCapIntensityCoord;
    MaterialProperty p_MatCapEmissionScaler;
    MaterialProperty p_MatCapDim;
    MaterialProperty p_MatCapHue;
    MaterialProperty p_MatCapSaturation;
    MaterialProperty p_MatCapContrast;
    MaterialProperty p_MatCapNormalOn;
    MaterialProperty p_MatCapNormalTex;
    MaterialProperty p_MatCapNormalSpeed;
    MaterialProperty p_MatCapNormalXCoord; MaterialProperty p_MatCapNormalYCoord;
    MaterialProperty p_MatCapNormalUVMode;
    MaterialProperty p_MatCapNormalIntensity;
    MaterialProperty p_MatCapNormalOffsetSpeedOn;

    // Depth Fade
    MaterialProperty p_DepthOn;
    MaterialProperty p_PreAOTDepthOn;
    MaterialProperty p_EdgeMode;
    MaterialProperty p_DepthSoftness;
    MaterialProperty p_DepthMethod;
    MaterialProperty p_DepthOpacityBlend;
    MaterialProperty p_EdgeColor;
    MaterialProperty p_EdgeContrast;
    MaterialProperty p_EdgeEffectColor;
    MaterialProperty p_CameraFadeOn;
    MaterialProperty p_CameraFadeNear;
    MaterialProperty p_CameraFadeFar;
    MaterialProperty p_CameraFadeWidth;

    void FindProperties(MaterialProperty[] props)
    {
        p_RenderPriority = FindProperty("_RenderPriority", props);
        p_AlwaysOnTop = FindProperty("_AlwaysOnTop", props);
        p_CullMode = FindProperty("_CullMode", props);
        p_DstBlendMode = FindProperty("_DstBlendMode", props);
        p_ZWriteMode = FindProperty("_ZWriteMode", props);
        p_UseCustomData = FindProperty("_UseCustomData", props);
        p_Saturation = FindProperty("_Saturation", props);
        p_Contrast = FindProperty("_Contrast", props);

        p_DiffuseTex1 = FindProperty("_DiffuseTex1", props);
        p_EmissionScaler = FindProperty("_EmissionScaler", props);
        p_ColorRGBA = FindProperty("_ColorRGBA", props);
        p_DiffuseTex1Speed = FindProperty("_DiffuseTex1Speed", props);
        p_Diff1OffsetXCoord = FindProperty("_Diff1OffsetXCoord", props);
        p_Diff1OffsetYCoord = FindProperty("_Diff1OffsetYCoord", props);
        p_Diff1UVMode = FindProperty("_Diff1UVMode", props);
        p_Diff1ChannelMode = FindProperty("_Diff1ChannelMode", props);
        p_Diff1Angle = FindProperty("_Diff1Angle", props);
        p_Diff1AngleCoord = FindProperty("_Diff1AngleCoord", props);
        p_Diff1AngleSpeed = FindProperty("_Diff1AngleSpeed", props);

        p_DiffuseTex2On = FindProperty("_DiffuseTex2On", props);
        p_Diff2BlendMode = FindProperty("_Diff2BlendMode", props);
        p_DiffuseTex2 = FindProperty("_DiffuseTex2", props);
        p_DiffuseTex2Speed = FindProperty("_DiffuseTex2Speed", props);
        p_Diff2OffsetXCoord = FindProperty("_Diff2OffsetXCoord", props);
        p_Diff2OffsetYCoord = FindProperty("_Diff2OffsetYCoord", props);
        p_Diff2UVMode = FindProperty("_Diff2UVMode", props);
        p_Diff2ChannelMode = FindProperty("_Diff2ChannelMode", props);
        p_Diff2Angle = FindProperty("_Diff2Angle", props);
        p_Diff2AngleCoord = FindProperty("_Diff2AngleCoord", props);
        p_Diff2AngleSpeed = FindProperty("_Diff2AngleSpeed", props);
        p_Diff2EmissionScaler = FindProperty("_Diff2EmissionScaler", props);
        p_Diff2Intensity = FindProperty("_Diff2Intensity", props);
        p_Diff2IntensityCoord = FindProperty("_Diff2IntensityCoord", props);

        p_DissolveOn = FindProperty("_DissolveOn", props);
        p_DissolveTex = FindProperty("_DissolveTex", props);
        p_DissSpeed = FindProperty("_DissSpeed", props);
        p_DissTexOffsetXCoord = FindProperty("_DissTexOffsetXCoord", props);
        p_DissTexOffsetYCoord = FindProperty("_DissTexOffsetYCoord", props);
        p_DissTexUVMode = FindProperty("_DissTexUVMode", props);
        p_DissolveTexChannel = FindProperty("_DissolveTexChannel", props);
        p_DissolveRangeCustom2x = FindProperty("_DissolveRangeCustom2x", props);
        p_DissolveRangeCoord = FindProperty("_DissolveRangeCoord", props);
        p_SmoothIntensityCustom2y = FindProperty("_SmoothIntensityCustom2y", props);
        p_SmoothIntensityCoord = FindProperty("_SmoothIntensityCoord", props);
        p_DissolveEdgeRange = FindProperty("_DissolveEdgeRange", props);
        p_DissolveEdgeRangeCoord = FindProperty("_DissolveEdgeRangeCoord", props);
        p_DissolveEdgeColor = FindProperty("_DissolveEdgeColor", props);
        p_DissolveBlendType = FindProperty("_DissolveBlendType", props);
        p_DissTexAngle = FindProperty("_DissTexAngle", props);
        p_DissTexAngleCoord = FindProperty("_DissTexAngleCoord", props);
        p_DissTexAngleSpeed = FindProperty("_DissTexAngleSpeed", props);
        p_DissolveMaskOn = FindProperty("_DissolveMaskOn", props);
        p_DissolveMask = FindProperty("_DissolveMask", props);
        p_DissolveMaskChannel = FindProperty("_DissolveMaskChannel", props);
        p_DissolveMaskIntensity = FindProperty("_DissolveMaskIntensity", props);
        p_DissolveMaskIntensityCoord = FindProperty("_DissolveMaskIntensityCoord", props);
        p_DissolveMaskSpeed = FindProperty("_DissolveMaskSpeed", props);
        p_DissMaskOffsetXCoord = FindProperty("_DissMaskOffsetXCoord", props);
        p_DissMaskOffsetYCoord = FindProperty("_DissMaskOffsetYCoord", props);
        p_DissMaskUVMode = FindProperty("_DissMaskUVMode", props);
        p_DissMaskAngle = FindProperty("_DissMaskAngle", props);
        p_DissMaskAngleCoord = FindProperty("_DissMaskAngleCoord", props);
        p_DissMaskAngleSpeed = FindProperty("_DissMaskAngleSpeed", props);

        p_MaskTexOn = FindProperty("_MaskTexOn", props);
        p_MaskTex = FindProperty("_MaskTex", props);
        p_MaskChannel = FindProperty("_MaskChannel", props);
        p_MaskIntensity = FindProperty("_MaskIntensity", props);
        p_MaskIntensityCoord = FindProperty("_MaskIntensityCoord", props);
        p_MaskSpeed = FindProperty("_MaskSpeed", props);
        p_MaskOffsetXCoord = FindProperty("_MaskOffsetXCoord", props);
        p_MaskOffsetYCoord = FindProperty("_MaskOffsetYCoord", props);
        p_MaskUVMode = FindProperty("_MaskUVMode", props);
        p_MaskAngle = FindProperty("_MaskAngle", props);
        p_MaskAngleCoord = FindProperty("_MaskAngleCoord", props);
        p_MaskAngleSpeed = FindProperty("_MaskAngleSpeed", props);

        p_NoiseTexOn = FindProperty("_NoiseTexOn", props);
        p_NoiseTex = FindProperty("_NoiseTex", props);
        p_NoiseSpeed = FindProperty("_NoiseSpeed", props);
        p_NoiseOffsetXCoord = FindProperty("_NoiseOffsetXCoord", props);
        p_NoiseOffsetYCoord = FindProperty("_NoiseOffsetYCoord", props);
        p_NoiseUVMode = FindProperty("_NoiseUVMode", props);
        p_NoiseTexChannel = FindProperty("_NoiseTexChannel", props);
        p_NoiseIntensityCustom2z = FindProperty("_NoiseIntensityCustom2z", props);
        p_NoiseIntensityCoord    = FindProperty("_NoiseIntensityCoord",    props);
        p_NoiseOffset = FindProperty("_NoiseOffset", props);
        p_NoiseDirection      = FindProperty("_NoiseDirection",      props);
        p_NoiseTargets        = FindProperty("_NoiseTargets",        props);
        p_UseUVMask = FindProperty("_UseUVMask", props);
        p_NoiseAngle = FindProperty("_NoiseAngle", props);
        p_NoiseAngleCoord = FindProperty("_NoiseAngleCoord", props);
        p_NoiseAngleSpeed = FindProperty("_NoiseAngleSpeed", props);
        p_NoiseUVMask = FindProperty("_NoiseUVMask", props);

        p_NoiseUVMaskOffsetXCoord = FindProperty("_NoiseUVMaskOffsetXCoord", props);
        p_NoiseUVMaskOffsetYCoord = FindProperty("_NoiseUVMaskOffsetYCoord", props);
        p_NoiseUVMaskUVMode = FindProperty("_NoiseUVMaskUVMode", props);
        p_NoiseUVMaskSpeed = FindProperty("_NoiseUVMaskSpeed", props);
        p_NoiseUVMaskAngle = FindProperty("_NoiseUVMaskAngle", props);
        p_NoiseUVMaskAngleCoord = FindProperty("_NoiseUVMaskAngleCoord", props);
        p_NoiseUVMaskAngleSpeed = FindProperty("_NoiseUVMaskAngleSpeed", props);
        p_NoiseUVMaskChannel = FindProperty("_NoiseUVMaskChannel", props);
        p_NoiseUVMaskIntensity = FindProperty("_NoiseUVMaskIntensity", props);
        p_NoiseUVMaskIntensityCoord = FindProperty("_NoiseUVMaskIntensityCoord", props);
        p_NoiseFlowMode      = FindProperty("_NoiseFlowMode",      props);
        p_NoiseFlowChannelU  = FindProperty("_NoiseFlowChannelU",  props);
        p_NoiseFlowChannelV  = FindProperty("_NoiseFlowChannelV",  props);

        p_VertexOffsetOn = FindProperty("_VertexOffsetOn", props);
        p_OffsetDirMode = FindProperty("_OffsetDirMode", props);
        p_VertexOffsetBaseValue = FindProperty("_VertexOffsetBaseValue", props);
        p_VertexOffsetTex = FindProperty("_VertexOffsetTex", props);
        p_VertexOffsetTexChannel = FindProperty("_VertexOffsetTexChannel", props);
        p_OffsetSpeed = FindProperty("_OffsetSpeed", props);
        p_VtxOffsetTexXCoord = FindProperty("_VtxOffsetTexXCoord", props);
        p_VtxOffsetTexYCoord = FindProperty("_VtxOffsetTexYCoord", props);
        p_VtxOffsetUVMode = FindProperty("_VtxOffsetUVMode", props);
        p_OffsetTexAngle = FindProperty("_OffsetTexAngle", props);
        p_OffsetTexAngleCoord = FindProperty("_OffsetTexAngleCoord", props);
        p_OffsetTexAngleSpeed = FindProperty("_OffsetTexAngleSpeed", props);
        p_VertexOffsetIntensityCustom2w = FindProperty("_VertexOffsetIntensityCustom2w", props);
        p_VtxIntensityCoord = FindProperty("_VtxIntensityCoord", props);
        p_UseOffsetMask = FindProperty("_UseOffsetMask", props);
        p_VertexOffsetMaskTex = FindProperty("_VertexOffsetMaskTex", props);
        p_VtxOffsetMaskChannel = FindProperty("_VtxOffsetMaskChannel", props);
        p_VtxOffsetMaskSpeed = FindProperty("_VtxOffsetMaskSpeed", props);
        p_VtxOffsetMaskXCoord = FindProperty("_VtxOffsetMaskXCoord", props);
        p_VtxOffsetMaskYCoord = FindProperty("_VtxOffsetMaskYCoord", props);
        p_VtxOffsetMaskUVMode = FindProperty("_VtxOffsetMaskUVMode", props);
        p_VtxOffsetMaskAngle = FindProperty("_VtxOffsetMaskAngle", props);
        p_VtxOffsetMaskAngleCoord = FindProperty("_VtxOffsetMaskAngleCoord", props);
        p_VtxOffsetMaskAngleSpeed = FindProperty("_VtxOffsetMaskAngleSpeed", props);
        p_VtxOffsetMaskIntensity = FindProperty("_VtxOffsetMaskIntensity", props);
        p_VtxOffsetMaskIntensityCoord = FindProperty("_VtxOffsetMaskIntensityCoord", props);
        p_OffsetDirectionXYZ = FindProperty("_OffsetDirectionXYZ", props);
        p_AxisScaleMode = FindProperty("_AxisScaleMode", props);

        p_Diff1WrapMode         = FindProperty("_Diff1WrapMode",         props);
        p_Diff1WrapModeU        = FindProperty("_Diff1WrapModeU",        props);
        p_Diff1WrapModeV        = FindProperty("_Diff1WrapModeV",        props);
        p_Diff2WrapMode         = FindProperty("_Diff2WrapMode",         props);
        p_Diff2WrapModeU        = FindProperty("_Diff2WrapModeU",        props);
        p_Diff2WrapModeV        = FindProperty("_Diff2WrapModeV",        props);
        p_DissTexWrapMode       = FindProperty("_DissTexWrapMode",       props);
        p_DissTexWrapModeU      = FindProperty("_DissTexWrapModeU",      props);
        p_DissTexWrapModeV      = FindProperty("_DissTexWrapModeV",      props);
        p_DissMaskWrapMode      = FindProperty("_DissMaskWrapMode",      props);
        p_DissMaskWrapModeU     = FindProperty("_DissMaskWrapModeU",     props);
        p_DissMaskWrapModeV     = FindProperty("_DissMaskWrapModeV",     props);
        p_MaskWrapMode          = FindProperty("_MaskWrapMode",          props);
        p_MaskWrapModeU         = FindProperty("_MaskWrapModeU",         props);
        p_MaskWrapModeV         = FindProperty("_MaskWrapModeV",         props);
        p_NoiseWrapMode         = FindProperty("_NoiseWrapMode",         props);
        p_NoiseWrapModeU        = FindProperty("_NoiseWrapModeU",        props);
        p_NoiseWrapModeV        = FindProperty("_NoiseWrapModeV",        props);
        p_NoiseUVMaskWrapMode   = FindProperty("_NoiseUVMaskWrapMode",   props);
        p_NoiseUVMaskWrapModeU  = FindProperty("_NoiseUVMaskWrapModeU",  props);
        p_NoiseUVMaskWrapModeV  = FindProperty("_NoiseUVMaskWrapModeV",  props);
        p_VtxOffsetWrapMode     = FindProperty("_VtxOffsetWrapMode",     props);
        p_VtxOffsetWrapModeU    = FindProperty("_VtxOffsetWrapModeU",    props);
        p_VtxOffsetWrapModeV    = FindProperty("_VtxOffsetWrapModeV",    props);
        p_VtxMaskWrapMode       = FindProperty("_VtxMaskWrapMode",       props);
        p_VtxMaskWrapModeU      = FindProperty("_VtxMaskWrapModeU",      props);
        p_VtxMaskWrapModeV      = FindProperty("_VtxMaskWrapModeV",      props);
        p_MatCapWrapMode        = FindProperty("_MatCapWrapMode",        props);
        p_MatCapWrapModeU       = FindProperty("_MatCapWrapModeU",       props);
        p_MatCapWrapModeV       = FindProperty("_MatCapWrapModeV",       props);
        p_MatCapNormalWrapMode  = FindProperty("_MatCapNormalWrapMode",  props);
        p_MatCapNormalWrapModeU = FindProperty("_MatCapNormalWrapModeU", props);
        p_MatCapNormalWrapModeV = FindProperty("_MatCapNormalWrapModeV", props);

        p_Diff1WrapOn        = FindProperty("_Diff1WrapOn",        props);
        p_Diff2WrapOn        = FindProperty("_Diff2WrapOn",        props);
        p_DissTexWrapOn      = FindProperty("_DissTexWrapOn",      props);
        p_DissMaskWrapOn     = FindProperty("_DissMaskWrapOn",     props);
        p_MaskWrapOn         = FindProperty("_MaskWrapOn",         props);
        p_NoiseWrapOn        = FindProperty("_NoiseWrapOn",        props);
        p_NoiseUVMaskWrapOn  = FindProperty("_NoiseUVMaskWrapOn",  props);
        p_VtxOffsetWrapOn    = FindProperty("_VtxOffsetWrapOn",    props);
        p_VtxMaskWrapOn      = FindProperty("_VtxMaskWrapOn",      props);
        p_MatCapWrapOn       = FindProperty("_MatCapWrapOn",       props);
        p_MatCapNormalWrapOn = FindProperty("_MatCapNormalWrapOn", props);

        p_Diff1OffsetSpeedOn      = FindProperty("_Diff1OffsetSpeedOn",      props);
        p_Diff1AngleSpeedOn       = FindProperty("_Diff1AngleSpeedOn",       props);
        p_Diff2OffsetSpeedOn      = FindProperty("_Diff2OffsetSpeedOn",      props);
        p_Diff2AngleSpeedOn       = FindProperty("_Diff2AngleSpeedOn",       props);
        p_DissTexOffsetSpeedOn    = FindProperty("_DissTexOffsetSpeedOn",    props);
        p_DissTexAngleSpeedOn     = FindProperty("_DissTexAngleSpeedOn",     props);
        p_DissMaskOffsetSpeedOn   = FindProperty("_DissMaskOffsetSpeedOn",   props);
        p_DissMaskAngleSpeedOn    = FindProperty("_DissMaskAngleSpeedOn",    props);
        p_MaskOffsetSpeedOn       = FindProperty("_MaskOffsetSpeedOn",       props);
        p_MaskAngleSpeedOn        = FindProperty("_MaskAngleSpeedOn",        props);
        p_NoiseOffsetSpeedOn      = FindProperty("_NoiseOffsetSpeedOn",      props);
        p_NoiseAngleSpeedOn       = FindProperty("_NoiseAngleSpeedOn",       props);
        p_NoiseUVMaskOffsetSpeedOn = FindProperty("_NoiseUVMaskOffsetSpeedOn", props);
        p_NoiseUVMaskAngleSpeedOn  = FindProperty("_NoiseUVMaskAngleSpeedOn",  props);
        p_VtxTexOffsetSpeedOn     = FindProperty("_VtxTexOffsetSpeedOn",     props);
        p_VtxTexAngleSpeedOn      = FindProperty("_VtxTexAngleSpeedOn",      props);
        p_VtxMaskOffsetSpeedOn    = FindProperty("_VtxMaskOffsetSpeedOn",    props);
        p_VtxMaskAngleSpeedOn     = FindProperty("_VtxMaskAngleSpeedOn",     props);

        p_RimMode             = FindProperty("_RimMode",             props);
        p_PreMatCapCullMode   = FindProperty("_PreMatCapCullMode",   props);
        p_FresnelProgress       = FindProperty("_FresnelProgress",       props);
        p_FresnelProgressCoord  = FindProperty("_FresnelProgressCoord",  props);
        p_FresnelSharpness      = FindProperty("_FresnelSharpness",      props);
        p_FresnelSharpnessCoord = FindProperty("_FresnelSharpnessCoord", props);
        p_FresnelIntensity      = FindProperty("_FresnelIntensity",      props);
        p_FresnelIntensityCoord = FindProperty("_FresnelIntensityCoord", props);
        p_FresnelColor     = FindProperty("_FresnelColor",      props);
        p_FresnelBlendMode = FindProperty("_FresnelBlendMode",  props);
        p_Inverse = FindProperty("_Inverse", props);
        p_UseFresnelAbs = FindProperty("_UseFresnelAbs", props);
        p_FresnelChannel = FindProperty("_FresnelChannel", props);
        p_MatCapTex              = FindProperty("_MatCapTex",               props);
        p_MatCapAngle            = FindProperty("_MatCapAngle",             props);
        p_MatCapAngleCoord       = FindProperty("_MatCapAngleCoord",        props);
        p_MatCapAngleSpeed       = FindProperty("_MatCapAngleSpeed",        props);
        p_MatCapAngleSpeedOn     = FindProperty("_MatCapAngleSpeedOn",      props);
        p_MatCapBlendMode        = FindProperty("_MatCapBlendMode",         props);
        p_MatCapColorMode        = FindProperty("_MatCapColorMode",          props);
        p_MatCapIntensity        = FindProperty("_MatCapIntensity",         props);
        p_MatCapIntensityCoord   = FindProperty("_MatCapIntensityCoord",    props);
        p_MatCapEmissionScaler   = FindProperty("_MatCapEmissionScaler",    props);
        p_MatCapDim              = FindProperty("_MatCapDim",               props);
        p_MatCapHue              = FindProperty("_MatCapHue",               props);
        p_MatCapSaturation       = FindProperty("_MatCapSaturation",        props);
        p_MatCapContrast         = FindProperty("_MatCapContrast",          props);
        p_MatCapNormalOn         = FindProperty("_MatCapNormalOn",          props);
        p_MatCapNormalTex        = FindProperty("_MatCapNormalTex",         props);
        p_MatCapNormalSpeed      = FindProperty("_MatCapNormalSpeed",       props);
        p_MatCapNormalXCoord     = FindProperty("_MatCapNormalXCoord",      props);
        p_MatCapNormalYCoord     = FindProperty("_MatCapNormalYCoord",      props);
        p_MatCapNormalUVMode     = FindProperty("_MatCapNormalUVMode",      props);
        p_MatCapNormalIntensity  = FindProperty("_MatCapNormalIntensity",   props);
        p_MatCapNormalOffsetSpeedOn = FindProperty("_MatCapNormalOffsetSpeedOn", props);

        p_DepthOn       = FindProperty("_DepthOn",       props);
        p_PreAOTDepthOn = FindProperty("_PreAOTDepthOn", props);
        p_EdgeMode      = FindProperty("_EdgeMode",      props);
        p_DepthSoftness  = FindProperty("_DepthSoftness",  props);
        p_DepthMethod    = FindProperty("_DepthMethod",    props);
        p_DepthOpacityBlend = FindProperty("_DepthOpacityBlend", props);
        p_EdgeColor       = FindProperty("_EdgeColor",       props);
        p_EdgeContrast    = FindProperty("_EdgeContrast",    props);
        p_EdgeEffectColor = FindProperty("_EdgeEffectColor", props);
        p_CameraFadeOn    = FindProperty("_CameraFadeOn",    props);
        p_CameraFadeNear  = FindProperty("_CameraFadeNear",  props);
        p_CameraFadeFar   = FindProperty("_CameraFadeFar",   props);
        p_CameraFadeWidth = FindProperty("_CameraFadeWidth", props);
    }

    // ────────────────────────────────────────────────────────────────────────
    //  Foldout
    // ────────────────────────────────────────────────────────────────────────
    static bool Foldout(bool display, string title, bool active = true)
    {
        var style = new GUIStyle("ShurikenModuleTitle");
        style.font = new GUIStyle(EditorStyles.boldLabel).font;
        style.border = new RectOffset(15, 7, 4, 4);
        style.fixedHeight = 22;
        style.contentOffset = new Vector2(20f, -2f);
        style.fontSize = 11;
        style.normal.textColor = active ? new Color(0.7f, 0.8f, 0.9f) : Color.white;

        var rect = GUILayoutUtility.GetRect(16f, 25f, style);
        GUI.Box(rect, title, style);

        var toggleRect = new Rect(rect.x + 4f, rect.y + 2f, 13f, 13f);
        if (Event.current.type == EventType.Repaint)
            EditorStyles.foldout.Draw(toggleRect, false, false, display, false);

        if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
        {
            display = !display;
            Event.current.Use();
        }
        return display;
    }

    // ────────────────────────────────────────────────────────────────────────
    //  ToggleLeft
    // ────────────────────────────────────────────────────────────────────────
    static void DisableKeywords(Material mat, params string[] kws)
    {
        foreach (var kw in kws) mat.DisableKeyword(kw);
    }

    // ────────────────────────────────────────────────────────────────────────
    static void DrawToggleLeft(MaterialEditor editor, MaterialProperty prop, string label)
    {
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = prop.hasMixedValue;
        bool val = EditorGUILayout.ToggleLeft(label, prop.floatValue == 1f);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            editor.RegisterPropertyChangeUndo(prop.name);
            prop.floatValue = val ? 1f : 0f;
            foreach (var t in prop.targets)
                MaterialEditor.ApplyMaterialPropertyDrawers(t as Material);
        }
    }

    static void DrawToggleRight(MaterialEditor editor, MaterialProperty prop, string label)
    {
        var row = EditorGUILayout.GetControlRect();
        float toggleX = row.x + EditorGUIUtility.labelWidth;
        EditorGUI.LabelField(new Rect(row.x, row.y, EditorGUIUtility.labelWidth - 4f, row.height), label);
        int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = prop.hasMixedValue;
        bool val = EditorGUI.Toggle(new Rect(toggleX, row.y, 14f, row.height), prop.floatValue > 0.5f);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        {
            editor.RegisterPropertyChangeUndo(prop.name);
            prop.floatValue = val ? 1f : 0f;
            foreach (var t in prop.targets)
                MaterialEditor.ApplyMaterialPropertyDrawers(t as Material);
        }
        EditorGUI.indentLevel = savedI;
    }

    // ────────────────────────────────────────────────────────────────────────
    //  Texture block: header row + [55x55 preview | Tiling / Offset / Offset Speed / CD Index / UV Options]
    //
    //  speedProp   : null = hide the Offset Speed row
    //  xCoordProp  : null = hide the CD Index row
    //  uvModeProp  : null = hide the UV Options row
    // ────────────────────────────────────────────────────────────────────────
    static readonly string[] CoordOptions =
        { "None", "CD1.x", "CD1.y", "CD1.z", "CD1.w", "CD2.x", "CD2.y", "CD2.z", "CD2.w" };

    static readonly string[] UVModeOptions = { "UV1", "UV2", "Screen" };

    static readonly int s_TexPickerHash = "TexPicker".GetHashCode();

    static void DrawTextureBlock(
        MaterialEditor editor, MaterialProperty texProp, string label,
        MaterialProperty uvModeProp,
        bool showTitle = true)
    {
        const float previewSize = 55f;
        const float gap = 6f;
        const float rowSpacing = 2f;
        float lineH = EditorGUIUtility.singleLineHeight;
        float step = lineH + rowSpacing;

        // Count rows on the right to determine the main row height.
        int rows = 2; // Tiling + Offset
        if (uvModeProp != null) rows++; // UV Options
        float rightH = rows * lineH + (rows - 1) * rowSpacing;
        float mainH = Mathf.Max(previewSize, rightH);

        // ── Header row (full width)──
        if (showTitle)
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);

        // ── Main row ──
        Rect main = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect(false, mainH));
        Rect sqRect = new Rect(main.x, main.y, previewSize, previewSize);
        float rx = main.x + previewSize + gap;
        float rw = main.width - previewSize - gap;
        float ry = main.y + (mainH - rightH) * 0.5f;

        // ─ Preview & interaction ─
        int id = GUIUtility.GetControlID(s_TexPickerHash, FocusType.Passive, sqRect);
        var evt = Event.current;

        // Checkerboard background.
        {
            Color ca = EditorGUIUtility.isProSkin ? new Color(0.33f, 0.33f, 0.33f) : new Color(0.60f, 0.60f, 0.60f);
            Color cb = EditorGUIUtility.isProSkin ? new Color(0.22f, 0.22f, 0.22f) : new Color(0.45f, 0.45f, 0.45f);
            const int cs = 8;
            for (int cy = 0; cy < (int)sqRect.height; cy += cs)
                for (int cx = 0; cx < (int)sqRect.width; cx += cs)
                {
                    bool lite = ((cx / cs) + (cy / cs)) % 2 == 0;
                    EditorGUI.DrawRect(new Rect(sqRect.x + cx, sqRect.y + cy,
                        Mathf.Min(cs, (int)sqRect.width - cx), Mathf.Min(cs, (int)sqRect.height - cy)),
                        lite ? ca : cb);
                }
        }
        if (texProp.textureValue != null)
            GUI.DrawTexture(sqRect, texProp.textureValue, ScaleMode.ScaleToFit, true);
        else
            GUI.Label(sqRect, "None\n(Texture 2D)",
                new GUIStyle(EditorStyles.centeredGreyMiniLabel) { wordWrap = true });

        // Border (blue when focused).
        var bc = GUIUtility.keyboardControl == id
            ? new Color(0.22f, 0.44f, 0.86f, 1f)
            : new Color(0.1f, 0.1f, 0.1f, 1f);
        EditorGUI.DrawRect(new Rect(sqRect.x, sqRect.y, sqRect.width, 1f), bc);
        EditorGUI.DrawRect(new Rect(sqRect.x, sqRect.yMax - 1f, sqRect.width, 1f), bc);
        EditorGUI.DrawRect(new Rect(sqRect.x, sqRect.y, 1f, sqRect.height), bc);
        EditorGUI.DrawRect(new Rect(sqRect.xMax - 1f, sqRect.y, 1f, sqRect.height), bc);

        // Select button (overlay at the bottom right).
        const float selW = 49f, selH = 12f;
        var selRect = new Rect(sqRect.xMax - selW + 1f, sqRect.yMax - selH, selW, selH);
        if (GUI.Button(selRect, "Select", EditorStyles.miniButton))
        {
            var sel = Selection.activeObject as Texture2D;
            if (sel != null)
            {
                editor.RegisterPropertyChangeUndo(texProp.name);
                texProp.textureValue = sel;
                editor.Repaint();
            }
        }

        // Click the texture area -> locate the texture in the Project window and acquire keyboard focus.
        if (evt.type == EventType.MouseDown && evt.button == 0 &&
            sqRect.Contains(evt.mousePosition) && !selRect.Contains(evt.mousePosition))
        {
            if (texProp.textureValue != null)
                EditorGUIUtility.PingObject(texProp.textureValue);
            GUIUtility.keyboardControl = id;
            evt.Use();
        }

        // Delete / Backspace -> clear the texture slot.
        if (evt.type == EventType.KeyDown && GUIUtility.keyboardControl == id &&
            (evt.keyCode == KeyCode.Delete || evt.keyCode == KeyCode.Backspace))
        {
            editor.RegisterPropertyChangeUndo(texProp.name);
            texProp.textureValue = null;
            editor.Repaint();
            evt.Use();
        }

        // Drag and drop.
        if (evt.type == EventType.DragUpdated && sqRect.Contains(evt.mousePosition))
        {
            bool hasT = false;
            foreach (var o in DragAndDrop.objectReferences) if (o is Texture) { hasT = true; break; }
            DragAndDrop.visualMode = hasT ? DragAndDropVisualMode.Link : DragAndDropVisualMode.Rejected;
            evt.Use();
        }
        if (evt.type == EventType.DragPerform && sqRect.Contains(evt.mousePosition))
        {
            DragAndDrop.AcceptDrag();
            foreach (var o in DragAndDrop.objectReferences)
                if (o is Texture t) { editor.RegisterPropertyChangeUndo(texProp.name); texProp.textureValue = t; break; }
            evt.Use();
        }

        // ─ Properties on the right: PrefixLabel scales proportionally with the Inspector width ─
        int savedIndent = EditorGUI.indentLevel;
        float savedLblW = EditorGUIUtility.labelWidth;
        EditorGUI.indentLevel = 0;
        EditorGUIUtility.labelWidth = Mathf.Max(38f, rw * 0.42f);

        // Tiling
        {
            Rect cont = EditorGUI.PrefixLabel(new Rect(rx, ry, rw, lineH), new GUIContent("Tiling"));
            Vector4 st = texProp.textureScaleAndOffset;
            float[] vals = { st.x, st.y };
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = texProp.hasMixedValue;
            EditorGUI.MultiFloatField(cont, new[] { new GUIContent("U"), new GUIContent("V") }, vals);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                editor.RegisterPropertyChangeUndo(texProp.name);
                var ns = texProp.textureScaleAndOffset; ns.x = vals[0]; ns.y = vals[1];
                texProp.textureScaleAndOffset = ns;
            }
            ry += step;
        }

        // Offset
        {
            Rect cont = EditorGUI.PrefixLabel(new Rect(rx, ry, rw, lineH), new GUIContent("Offset"));
            Vector4 st = texProp.textureScaleAndOffset;
            float[] vals = { st.z, st.w };
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = texProp.hasMixedValue;
            EditorGUI.MultiFloatField(cont, new[] { new GUIContent("U"), new GUIContent("V") }, vals);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                editor.RegisterPropertyChangeUndo(texProp.name);
                var ns = texProp.textureScaleAndOffset; ns.z = vals[0]; ns.w = vals[1];
                texProp.textureScaleAndOffset = ns;
            }
            ry += step;
        }

        // UV Options
        if (uvModeProp != null)
        {
            Rect cont = EditorGUI.PrefixLabel(new Rect(rx, ry, rw, lineH), new GUIContent("UV Options"));
            int uvi = Mathf.Clamp((int)uvModeProp.floatValue, 0, UVModeOptions.Length - 1);
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = uvModeProp.hasMixedValue;
            uvi = EditorGUI.Popup(cont, uvi, UVModeOptions);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            {
                editor.RegisterPropertyChangeUndo(uvModeProp.name);
                uvModeProp.floatValue = uvi;
            }
        }

        EditorGUI.indentLevel = savedIndent;
        EditorGUIUtility.labelWidth = savedLblW;
    }

    // Diffuse 1 / 2 Texture Channel: whether the layer provides color, Alpha, or both (values match DiffuseChannel in the shader).
    static readonly string[] kDiffuseChannelLabels = { "RGBA", "RGB", "R", "A", "R2A" };

    static void DrawDiffuseChannelRow(MaterialEditor editor, MaterialProperty prop)
    {
        var row = EditorGUILayout.GetControlRect();
        var cont = EditorGUI.PrefixLabel(row, new GUIContent("Texture Channel",
            "RGBA: color + alpha.  RGB: color only.  R: R as grayscale color.  A: alpha only.  R2A: R as alpha."));
        int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
        int mode = Mathf.Clamp((int)prop.floatValue, 0, kDiffuseChannelLabels.Length - 1);
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = prop.hasMixedValue;
        mode = EditorGUI.Popup(cont, mode, kDiffuseChannelLabels);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        { editor.RegisterPropertyChangeUndo(prop.name); prop.floatValue = mode; }
        EditorGUI.indentLevel = savedI;
    }

    static readonly string[] kWrapLabels     = { "Repeat", "Clamp", "Mirror", "Mirror Once", "Per-axis" };
    static readonly string[] kWrapAxisLabels = { "Repeat", "Clamp", "Mirror", "Mirror Once" };
    // wrapMode values 1-5 <-> popup indices 0-4 (None is represented by the wrapOn master toggle on the right, so it is omitted from the dropdown).

    static void DrawWrapModeRow(MaterialEditor editor, MaterialProperty wrapProp,
                                MaterialProperty wrapPropU, MaterialProperty wrapPropV,
                                MaterialProperty wrapOnProp)
    {
        const float tW = 14f;
        int savedI = EditorGUI.indentLevel;
        bool on = wrapOnProp == null || wrapOnProp.floatValue > 0.5f;

        var row = EditorGUILayout.GetControlRect();
        var cont = EditorGUI.PrefixLabel(row, new GUIContent("Wrap Mode"));
        EditorGUI.indentLevel = 0;

        // Main dropdown (disabled when off); reserve space for the toggle on the right.
        var popupRect = new Rect(cont.x, cont.y, cont.width - tW - 2f, cont.height);
        EditorGUI.BeginDisabledGroup(!on);
        int wm = Mathf.Clamp((int)wrapProp.floatValue, 1, 5) - 1;
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = wrapProp.hasMixedValue;
        wm = EditorGUI.Popup(popupRect, wm, kWrapLabels);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        { editor.RegisterPropertyChangeUndo(wrapProp.name); wrapProp.floatValue = wm + 1; }
        EditorGUI.EndDisabledGroup();

        // Master toggle on the right (on by default; off = None, using the texture Import Settings).
        if (wrapOnProp != null)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = wrapOnProp.hasMixedValue;
            bool newOn = EditorGUI.Toggle(new Rect(row.xMax - tW, row.y, tW, row.height), on);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            { editor.RegisterPropertyChangeUndo(wrapOnProp.name); wrapOnProp.floatValue = newOn ? 1f : 0f; }
        }

        // Per-axis subrows (visible only when enabled and mode == Per-axis).
        if (on && (int)wrapProp.floatValue == 5)
        {
            EditorGUI.indentLevel = savedI;
            var rowU = EditorGUILayout.GetControlRect();
            var contU = EditorGUI.PrefixLabel(rowU, new GUIContent("   U"));
            EditorGUI.indentLevel = 0;
            int wu = Mathf.Clamp((int)wrapPropU.floatValue, 1, 4) - 1;
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = wrapPropU.hasMixedValue;
            wu = EditorGUI.Popup(contU, wu, kWrapAxisLabels);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            { editor.RegisterPropertyChangeUndo(wrapPropU.name); wrapPropU.floatValue = wu + 1; }

            EditorGUI.indentLevel = savedI;
            var rowV = EditorGUILayout.GetControlRect();
            var contV = EditorGUI.PrefixLabel(rowV, new GUIContent("   V"));
            EditorGUI.indentLevel = 0;
            int wv = Mathf.Clamp((int)wrapPropV.floatValue, 1, 4) - 1;
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = wrapPropV.hasMixedValue;
            wv = EditorGUI.Popup(contV, wv, kWrapAxisLabels);
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            { editor.RegisterPropertyChangeUndo(wrapPropV.name); wrapPropV.floatValue = wv + 1; }
        }

        EditorGUI.indentLevel = savedI;
    }

    // Angle row: slider on the left + CD Index dropdown on the right (no label).
    void DrawAngleWithCoord(MaterialEditor editor, MaterialProperty angleProp,
                            MaterialProperty coordProp, bool cdOn)
    {
        var full = EditorGUILayout.GetControlRect();
        var contents = full;
        contents.xMin += EditorGUIUtility.labelWidth;

        const float fieldW = 35f;
        float coordW = contents.width / 3f;

        var coordRect = new Rect(full.xMax - coordW, full.y, coordW, full.height);
        var fieldRect = new Rect(coordRect.xMin - fieldW, full.y, fieldW, full.height);
        var sliderRect = new Rect(contents.xMin, full.y, fieldRect.xMin - contents.xMin - 4f, full.height);
        var labelRect = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);

        int ci = Mathf.Clamp((int)coordProp.floatValue, 0, CoordOptions.Length - 1);
        DrawCDConflictLabel(labelRect, "Angle", ci, cdOn);

        int savedIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        EditorGUI.BeginChangeCheck();
        float sliderAngle = Mathf.Round(GUI.HorizontalSlider(sliderRect, angleProp.floatValue, 0f, 360f));
        if (EditorGUI.EndChangeCheck())
        { editor.RegisterPropertyChangeUndo(angleProp.name); angleProp.floatValue = sliderAngle; }
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = angleProp.hasMixedValue;
        float fieldAngle = EditorGUI.FloatField(fieldRect, angleProp.floatValue);
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        { editor.RegisterPropertyChangeUndo(angleProp.name); angleProp.floatValue = Mathf.Clamp(fieldAngle, 0f, 360f); }

        EditorGUI.BeginDisabledGroup(!cdOn);
        EditorGUI.BeginChangeCheck();
        EditorGUI.showMixedValue = coordProp.hasMixedValue;
        if (CDConflict(ci)) GUI.contentColor = s_ConflictRed;
        ci = EditorGUI.Popup(coordRect, ci, CoordOptions);
        GUI.contentColor = Color.white;
        EditorGUI.showMixedValue = false;
        if (EditorGUI.EndChangeCheck())
        { editor.RegisterPropertyChangeUndo(coordProp.name); coordProp.floatValue = ci; }
        EditorGUI.EndDisabledGroup();

        EditorGUI.indentLevel = savedIndent;
    }

    // UV Options HelpBox: CD Index + Offset Speed (optional) + Angle (with CD popup) + Angle Speed.
    void DrawTexUVOptionsBox(MaterialEditor editor, bool cdOn,
                                    MaterialProperty xCoordProp, MaterialProperty yCoordProp,
                                    MaterialProperty speedProp,
                                    MaterialProperty angleProp,
                                    MaterialProperty angleCoordProp,
                                    MaterialProperty angleSpeedProp,
                                    MaterialProperty offsetSpeedOnProp = null,
                                    MaterialProperty angleSpeedOnProp  = null)
    {
        const float tW = 14f;
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        // CD Index
        {
            var row = EditorGUILayout.GetControlRect();
            int xi = Mathf.Clamp((int)xCoordProp.floatValue, 0, CoordOptions.Length - 1);
            int yi = Mathf.Clamp((int)yCoordProp.floatValue, 0, CoordOptions.Length - 1);
            bool xC = CDConflict(xi), yC = CDConflict(yi);

            // cont matches PrefixLabel: the field area starts at row.x + labelWidth.
            var cont = new Rect(row.x + EditorGUIUtility.labelWidth, row.y,
                                row.width - EditorGUIUtility.labelWidth, row.height);
            if (!xC && !yC)
            {
                EditorGUI.LabelField(new Rect(row.x, row.y, EditorGUIUtility.labelWidth, row.height),
                                     "CD Index");
            }
            else
            {
                var sb = new System.Text.StringBuilder();
                if (xC) sb.AppendLine("U conflict: " + string.Join(", ", _cdUsers[xi]));
                if (yC) sb.Append("V conflict: " + string.Join(", ", _cdUsers[yi]));
                var ic = EditorGUIUtility.IconContent("console.warnicon.sml");
                ic.tooltip = sb.ToString().TrimEnd();
                var indented = EditorGUI.IndentedRect(new Rect(row.x, row.y, EditorGUIUtility.labelWidth, row.height));
                int tmpI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                GUI.Label(new Rect(indented.x - 16f, row.y + 1f, 16f, 16f), ic);
                var red = new GUIStyle(EditorStyles.label)
                {
                    normal = { textColor = s_ConflictRed },
                    hover  = { textColor = s_ConflictRed },
                };
                if (!cdOn) EditorGUI.BeginDisabledGroup(true);
                GUI.Label(indented, "CD Index", red);
                if (!cdOn) EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel = tmpI;
            }

            const float xyLblW = 12f;
            float popW = (cont.width - xyLblW * 2 - 4f) * 0.5f;
            float cx = cont.x;
            Rect xLbl = new Rect(cx, row.y, xyLblW, row.height); cx += xyLblW;
            Rect xPop = new Rect(cx, row.y, popW, row.height); cx += popW + 4f;
            Rect yLbl = new Rect(cx, row.y, xyLblW, row.height); cx += xyLblW;
            Rect yPop = new Rect(cx, row.y, popW, row.height);

            EditorGUI.BeginDisabledGroup(!cdOn);
            int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
            EditorGUI.LabelField(xLbl, "U");
            EditorGUI.LabelField(yLbl, "V");
            EditorGUI.BeginChangeCheck();
            if (xC) GUI.contentColor = s_ConflictRed;
            xi = EditorGUI.Popup(xPop, xi, CoordOptions);
            GUI.contentColor = Color.white;
            if (EditorGUI.EndChangeCheck())
            { editor.RegisterPropertyChangeUndo(xCoordProp.name); xCoordProp.floatValue = xi; }
            EditorGUI.BeginChangeCheck();
            if (yC) GUI.contentColor = s_ConflictRed;
            yi = EditorGUI.Popup(yPop, yi, CoordOptions);
            GUI.contentColor = Color.white;
            if (EditorGUI.EndChangeCheck())
            { editor.RegisterPropertyChangeUndo(yCoordProp.name); yCoordProp.floatValue = yi; }
            EditorGUI.indentLevel = savedI;
            EditorGUI.EndDisabledGroup();
        }
        if (speedProp != null)
        {
            var row = EditorGUILayout.GetControlRect();
            int savedI = EditorGUI.indentLevel;
            bool spOn = offsetSpeedOnProp == null || offsetSpeedOnProp.floatValue > 0.5f;
            EditorGUI.PrefixLabel(row, new GUIContent("Offset Speed"));
            EditorGUI.indentLevel = 0;

            // Align exactly with the CD Index row above: contentX and numW use the same calculations.
            float contentX    = row.x + EditorGUIUtility.labelWidth;
            float contW       = row.width - EditorGUIUtility.labelWidth;
            const float uvLblW = 12f;
            float numW        = (contW - uvLblW * 2f - 4f) * 0.5f;
            float toggleSpace = offsetSpeedOnProp != null ? tW + 2f : 0f;
            float vNumW       = contW - toggleSpace - uvLblW * 2f - numW - 4f;

            EditorGUI.BeginDisabledGroup(!spOn);
            float[] vals = { speedProp.vectorValue.x, speedProp.vectorValue.y };
            EditorGUI.BeginChangeCheck();
            EditorGUI.showMixedValue = speedProp.hasMixedValue;
            float savedLblW = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = uvLblW;
            vals[0] = EditorGUI.FloatField(new Rect(contentX,                      row.y, uvLblW + numW,  row.height), new GUIContent("U"), vals[0]);
            vals[1] = EditorGUI.FloatField(new Rect(contentX + uvLblW + numW + 4f, row.y, uvLblW + vNumW, row.height), new GUIContent("V"), vals[1]);
            EditorGUIUtility.labelWidth = savedLblW;
            EditorGUI.showMixedValue = false;
            if (EditorGUI.EndChangeCheck())
            { editor.RegisterPropertyChangeUndo(speedProp.name); speedProp.vectorValue = new Vector4(vals[0], vals[1], 0f, 0f); }
            EditorGUI.EndDisabledGroup();
            if (offsetSpeedOnProp != null)
            {
                EditorGUI.BeginChangeCheck();
                bool newSpOn = EditorGUI.Toggle(new Rect(row.xMax - tW, row.y, tW, row.height), spOn);
                if (EditorGUI.EndChangeCheck())
                { editor.RegisterPropertyChangeUndo(offsetSpeedOnProp.name); offsetSpeedOnProp.floatValue = newSpOn ? 1f : 0f; }
            }
            EditorGUI.indentLevel = savedI;
        }
        if (angleProp != null)
        {
            DrawAngleWithCoord(editor, angleProp, angleCoordProp, cdOn);
            {
                var row = EditorGUILayout.GetControlRect();
                int savedI = EditorGUI.indentLevel;
                bool asOn = angleSpeedOnProp == null || angleSpeedOnProp.floatValue > 0.5f;
                float toggleW = angleSpeedOnProp != null ? tW + 2f : 0f;
                var fieldRow = new Rect(row.x, row.y, row.width - toggleW, row.height);
                EditorGUI.BeginDisabledGroup(!asOn);
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = angleSpeedProp.hasMixedValue;
                float asVal = EditorGUI.FloatField(fieldRow, new GUIContent("Angle Speed"), angleSpeedProp.floatValue);
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck())
                { editor.RegisterPropertyChangeUndo(angleSpeedProp.name); angleSpeedProp.floatValue = asVal; }
                EditorGUI.EndDisabledGroup();
                if (angleSpeedOnProp != null)
                {
                    EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    bool newAsOn = EditorGUI.Toggle(new Rect(row.xMax - tW, row.y, tW, row.height), asOn);
                    if (EditorGUI.EndChangeCheck())
                    { editor.RegisterPropertyChangeUndo(angleSpeedOnProp.name); angleSpeedOnProp.floatValue = newAsOn ? 1f : 0f; }
                }
                EditorGUI.indentLevel = savedI;
            }
        }
        EditorGUILayout.EndVertical();
    }

    // ────────────────────────────────────────────────────────────────────────
    //  OnGUI
    // ────────────────────────────────────────────────────────────────────────
    public override void OnGUI(MaterialEditor editor, MaterialProperty[] props)
    {
        FindProperties(props);
        m_Editor = editor;
        Material mat = editor.target as Material;
        bool cdOn = mat.IsKeywordEnabled("_USECUSTOMDATA_ON");
        _cdUsers = BuildCDUsers(mat);
        RefreshParticleRenderers(mat);

        // ── Render Options ──────────────────────────────────────────────────
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _RenderFoldout = Foldout(_RenderFoldout, "Render Options");
        if (_RenderFoldout)
        {
            EditorGUI.indentLevel++;
            GUILayout.Space(4);
            m_Editor.ShaderProperty(p_EmissionScaler, new GUIContent("Emission Scaler",
                "HDR brightness multiplier. 1 = original brightness."));
            m_Editor.ShaderProperty(p_ColorRGBA, "Tint Color");
            m_Editor.ShaderProperty(p_Saturation, "Saturation");
            m_Editor.ShaderProperty(p_Contrast, "Contrast");
            GUILayout.Space(4);
            bool _aotBefore = Mathf.RoundToInt(p_AlwaysOnTop.floatValue) == 0;
            EditorGUI.BeginChangeCheck();
            m_Editor.ShaderProperty(p_AlwaysOnTop, "Always On Top");
            if (EditorGUI.EndChangeCheck())
            {
                bool _aotAfter = Mathf.RoundToInt(p_AlwaysOnTop.floatValue) == 0;
                if (_aotAfter && !_aotBefore)
                {
                    p_PreAOTDepthOn.floatValue = p_DepthOn.floatValue;
                    p_DepthOn.floatValue = 0f;
                    foreach (var t in p_DepthOn.targets)
                        MaterialEditor.ApplyMaterialPropertyDrawers(t as Material);
                }
                else if (!_aotAfter && _aotBefore)
                {
                    float _saved = p_PreAOTDepthOn.floatValue;
                    if (_saved >= 0f)
                    {
                        p_DepthOn.floatValue = _saved;
                        foreach (var t in p_DepthOn.targets)
                            MaterialEditor.ApplyMaterialPropertyDrawers(t as Material);
                    }
                    p_PreAOTDepthOn.floatValue = -1f;
                }
            }
            bool _mcLock = Mathf.RoundToInt(mat.GetFloat("_RimMode")) == 2;
            if (_mcLock && p_CullMode.floatValue != 2f) p_CullMode.floatValue = 2f;
            EditorGUI.BeginDisabledGroup(_mcLock);
            m_Editor.ShaderProperty(p_CullMode, _mcLock ? "Facing  (locked by MatCap)" : "Facing");
            EditorGUI.EndDisabledGroup();
            m_Editor.ShaderProperty(p_DstBlendMode, "Blend Mode");
            m_Editor.ShaderProperty(p_ZWriteMode, "ZWrite");
            EditorGUI.BeginChangeCheck();
            m_Editor.ShaderProperty(p_RenderPriority, "Render Priority");
            if (EditorGUI.EndChangeCheck())
            {
                foreach (var t in p_RenderPriority.targets)
                    if (t is Material m) m.renderQueue = 3000 + (int)p_RenderPriority.floatValue;
            }
            GUILayout.Space(4);
            DrawToggleLeft(m_Editor, p_UseCustomData, "Enable Custom Data");
            GUILayout.Space(4);
            DrawVertexStreamCheck(mat, cdOn);
            if (cdOn)
                DrawCustomDataSummary(mat);
            GUILayout.Space(4);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();

        // ── Diffuse 1 ──────────────────────────────────────────────────────
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _Diff1Foldout = Foldout(_Diff1Foldout, "Diffuse 1", p_DiffuseTex1.textureValue != null);
        if (_Diff1Foldout)
        {
            EditorGUI.indentLevel++;
            GUILayout.Space(4);
            DrawTextureBlock(m_Editor, p_DiffuseTex1, "Diffuse Tex 1", p_Diff1UVMode);
            DrawWrapModeRow(m_Editor, p_Diff1WrapMode, p_Diff1WrapModeU, p_Diff1WrapModeV, p_Diff1WrapOn);
            DrawDiffuseChannelRow(m_Editor, p_Diff1ChannelMode);
            DrawTexUVOptionsBox(m_Editor, cdOn, p_Diff1OffsetXCoord, p_Diff1OffsetYCoord,
                p_DiffuseTex1Speed, p_Diff1Angle, p_Diff1AngleCoord, p_Diff1AngleSpeed,
                p_Diff1OffsetSpeedOn, p_Diff1AngleSpeedOn);
            GUILayout.Space(4);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();

        // ── Diffuse 2 ──────────────────────────────────────────────────────
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _Diff2Foldout = Foldout(_Diff2Foldout, "Diffuse 2", mat.GetFloat("_DiffuseTex2On") == 1f);
        if (_Diff2Foldout)
        {
            EditorGUI.indentLevel++;
            GUILayout.Space(4);
            EditorGUI.BeginChangeCheck();
            DrawToggleLeft(m_Editor, p_DiffuseTex2On, "Enable");
            if (EditorGUI.EndChangeCheck() && p_DiffuseTex2On.floatValue == 0f)
                foreach (var t in p_DiffuseTex2On.targets)
                    DisableKeywords(t as Material, "_DIFF2BLENDMODE_MULTIPLY", "_DIFF2BLENDMODE_ADD");
            if (mat.GetFloat("_DiffuseTex2On") == 1f)
            {
                GUILayout.Space(4);
                m_Editor.ShaderProperty(p_Diff2BlendMode, "Blend Mode");
                GUILayout.Space(4);
                DrawTextureBlock(m_Editor, p_DiffuseTex2, "Diffuse Tex 2", p_Diff2UVMode);
                DrawWrapModeRow(m_Editor, p_Diff2WrapMode, p_Diff2WrapModeU, p_Diff2WrapModeV, p_Diff2WrapOn);
                DrawDiffuseChannelRow(m_Editor, p_Diff2ChannelMode);
                DrawTexUVOptionsBox(m_Editor, cdOn, p_Diff2OffsetXCoord, p_Diff2OffsetYCoord,
                    p_DiffuseTex2Speed, p_Diff2Angle, p_Diff2AngleCoord, p_Diff2AngleSpeed,
                    p_Diff2OffsetSpeedOn, p_Diff2AngleSpeedOn);
                GUILayout.Space(2);
                {
                    var full = EditorGUILayout.GetControlRect();
                    var esLabelRect  = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    var esFieldRect  = new Rect(full.xMax - 35f, full.y, 35f, full.height);
                    var esSliderRect = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, esFieldRect.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                    EditorGUI.LabelField(esLabelRect, "Emission Scaler");
                    int savedI2 = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    float esSVal = GUI.HorizontalSlider(esSliderRect, p_Diff2EmissionScaler.floatValue, 1f, 50f);
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_Diff2EmissionScaler.name); p_Diff2EmissionScaler.floatValue = Mathf.Clamp(esSVal, 1f, 50f); }
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_Diff2EmissionScaler.hasMixedValue;
                    float esFVal = EditorGUI.FloatField(esFieldRect, p_Diff2EmissionScaler.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_Diff2EmissionScaler.name); p_Diff2EmissionScaler.floatValue = Mathf.Clamp(esFVal, 1f, 50f); }
                    EditorGUI.indentLevel = savedI2;
                }
                {
                    var full = EditorGUILayout.GetControlRect();
                    float coordW = (full.width - EditorGUIUtility.labelWidth) / 3f;
                    var coordRect = new Rect(full.xMax - coordW, full.y, coordW, full.height);
                    var fieldRect = new Rect(coordRect.xMin - 35f, full.y, 35f, full.height);
                    var sliderRect = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fieldRect.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                    var labelRect = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    int ci = Mathf.Clamp((int)p_Diff2IntensityCoord.floatValue, 0, CoordOptions.Length - 1);
                    DrawCDConflictLabel(labelRect, "Diffuse 2 Intensity", ci, cdOn);
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    float d2sVal = GUI.HorizontalSlider(sliderRect, p_Diff2Intensity.floatValue, 0f, 1f);
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_Diff2Intensity.name); p_Diff2Intensity.floatValue = Mathf.Clamp01(d2sVal); }
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_Diff2Intensity.hasMixedValue;
                    float d2fVal = EditorGUI.FloatField(fieldRect, p_Diff2Intensity.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_Diff2Intensity.name); p_Diff2Intensity.floatValue = Mathf.Clamp01(d2fVal); }
                    EditorGUI.BeginDisabledGroup(!cdOn);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_Diff2IntensityCoord.hasMixedValue;
                    if (CDConflict(ci)) GUI.contentColor = s_ConflictRed;
                    ci = EditorGUI.Popup(coordRect, ci, CoordOptions);
                    GUI.contentColor = Color.white;
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_Diff2IntensityCoord.name); p_Diff2IntensityCoord.floatValue = ci; }
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel = savedI;
                }
            }
            GUILayout.Space(4);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();

        // ── Mask ────────────────────────────────────────────────────────────
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _MaskFoldout = Foldout(_MaskFoldout, "Mask", mat.GetFloat("_MaskTexOn") == 1f);
        if (_MaskFoldout)
        {
            EditorGUI.indentLevel++;
            GUILayout.Space(4);
            DrawToggleLeft(m_Editor, p_MaskTexOn, "Enable");
            if (mat.GetFloat("_MaskTexOn") == 1f)
            {
                GUILayout.Space(4);
                DrawTextureBlock(m_Editor, p_MaskTex, "Mask Tex", p_MaskUVMode);
                DrawWrapModeRow(m_Editor, p_MaskWrapMode, p_MaskWrapModeU, p_MaskWrapModeV, p_MaskWrapOn);
                {
                    var row = EditorGUILayout.GetControlRect();
                    var cont = EditorGUI.PrefixLabel(row, new GUIContent("Texture Channel"));
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    int ch = Mathf.Clamp((int)p_MaskChannel.floatValue, 0, 3);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_MaskChannel.hasMixedValue;
                    ch = EditorGUI.Popup(cont, ch, new[] { "R", "G", "B", "A" });
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MaskChannel.name); p_MaskChannel.floatValue = ch; }
                    EditorGUI.indentLevel = savedI;
                }
                DrawTexUVOptionsBox(m_Editor, cdOn, p_MaskOffsetXCoord, p_MaskOffsetYCoord,
                    p_MaskSpeed, p_MaskAngle, p_MaskAngleCoord, p_MaskAngleSpeed,
                    p_MaskOffsetSpeedOn, p_MaskAngleSpeedOn);
                {
                    var full = EditorGUILayout.GetControlRect();
                    float coordW = (full.width - EditorGUIUtility.labelWidth) / 3f;
                    var coordRect = new Rect(full.xMax - coordW, full.y, coordW, full.height);
                    var fieldRect = new Rect(coordRect.xMin - 35f, full.y, 35f, full.height);
                    var sliderRect = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fieldRect.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                    var labelRect = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    int ci = Mathf.Clamp((int)p_MaskIntensityCoord.floatValue, 0, CoordOptions.Length - 1);
                    DrawCDConflictLabel(labelRect, "Mask Intensity", ci, cdOn);
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    float msVal = GUI.HorizontalSlider(sliderRect, p_MaskIntensity.floatValue, 0f, 1f);
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MaskIntensity.name); p_MaskIntensity.floatValue = Mathf.Clamp01(msVal); }
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_MaskIntensity.hasMixedValue;
                    float mfVal = EditorGUI.FloatField(fieldRect, p_MaskIntensity.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MaskIntensity.name); p_MaskIntensity.floatValue = Mathf.Clamp01(mfVal); }
                    EditorGUI.BeginDisabledGroup(!cdOn);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_MaskIntensityCoord.hasMixedValue;
                    if (CDConflict(ci)) GUI.contentColor = s_ConflictRed;
                    ci = EditorGUI.Popup(coordRect, ci, CoordOptions);
                    GUI.contentColor = Color.white;
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MaskIntensityCoord.name); p_MaskIntensityCoord.floatValue = ci; }
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel = savedI;
                }
            }
            GUILayout.Space(4);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();

        // ── Dissolve ────────────────────────────────────────────────────────
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _DissolveFoldout = Foldout(_DissolveFoldout, "Dissolve", mat.GetFloat("_DissolveOn") == 1f);
        if (_DissolveFoldout)
        {
            EditorGUI.indentLevel++;
            GUILayout.Space(4);
            EditorGUI.BeginChangeCheck();
            DrawToggleLeft(m_Editor, p_DissolveOn, "Enable");
            if (EditorGUI.EndChangeCheck() && p_DissolveOn.floatValue == 0f)
                foreach (var t in p_DissolveOn.targets)
                    DisableKeywords(t as Material, "_DISSOLVEMASKON_ON", "_DISSOLVEBLENDTYPE_ADD", "_DISSOLVEBLENDTYPE_MULTIPLY", "_DISSOLVEBLENDTYPE_TINT", "_DISSOLVEBLENDTYPE_SELECT");
            if (mat.GetFloat("_DissolveOn") == 1f)
            {
                GUILayout.Space(4);
                DrawTextureBlock(m_Editor, p_DissolveTex, "Dissolve Tex", p_DissTexUVMode);
                DrawWrapModeRow(m_Editor, p_DissTexWrapMode, p_DissTexWrapModeU, p_DissTexWrapModeV, p_DissTexWrapOn);
                {
                    var row = EditorGUILayout.GetControlRect();
                    var cont = EditorGUI.PrefixLabel(row, new GUIContent("Texture Channel"));
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    int ch = Mathf.Clamp((int)p_DissolveTexChannel.floatValue, 0, 3);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_DissolveTexChannel.hasMixedValue;
                    ch = EditorGUI.Popup(cont, ch, new[] { "R", "G", "B", "A" });
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_DissolveTexChannel.name); p_DissolveTexChannel.floatValue = ch; }
                    EditorGUI.indentLevel = savedI;
                }
                DrawTexUVOptionsBox(m_Editor, cdOn, p_DissTexOffsetXCoord, p_DissTexOffsetYCoord,
                    p_DissSpeed, p_DissTexAngle, p_DissTexAngleCoord, p_DissTexAngleSpeed,
                    p_DissTexOffsetSpeedOn, p_DissTexAngleSpeedOn);
                GUILayout.Space(4);
                {
                    var full = EditorGUILayout.GetControlRect();
                    float coordW = (full.width - EditorGUIUtility.labelWidth) / 3f;
                    var labelRect = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    var coordRect = new Rect(full.xMax - coordW, full.y, coordW, full.height);
                    var floatRow = new Rect(full.x, full.y, full.width - coordW, full.height);
                    int drCi = Mathf.Clamp((int)p_DissolveRangeCoord.floatValue, 0, CoordOptions.Length - 1);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_DissolveRangeCustom2x.hasMixedValue;
                    float drVal = EditorGUI.FloatField(floatRow, new GUIContent("Dissolve Range"), p_DissolveRangeCustom2x.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_DissolveRangeCustom2x.name); p_DissolveRangeCustom2x.floatValue = drVal; }
                    if (CDConflict(drCi)) DrawCDConflictLabel(labelRect, "Dissolve Range", drCi, cdOn);
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginDisabledGroup(!cdOn);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_DissolveRangeCoord.hasMixedValue;
                    if (CDConflict(drCi)) GUI.contentColor = s_ConflictRed;
                    drCi = EditorGUI.Popup(coordRect, drCi, CoordOptions);
                    GUI.contentColor = Color.white;
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_DissolveRangeCoord.name); p_DissolveRangeCoord.floatValue = drCi; }
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel = savedI;
                }
                {
                    var full = EditorGUILayout.GetControlRect();
                    float coordW = (full.width - EditorGUIUtility.labelWidth) / 3f;
                    var coordRect = new Rect(full.xMax - coordW, full.y, coordW, full.height);
                    var fieldRect = new Rect(coordRect.xMin - 35f, full.y, 35f, full.height);
                    var sliderRect = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fieldRect.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                    var labelRect = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    int siCi = Mathf.Clamp((int)p_SmoothIntensityCoord.floatValue, 0, CoordOptions.Length - 1);
                    DrawCDConflictLabel(labelRect, "Smooth Intensity", siCi, cdOn);
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    float siSVal = GUI.HorizontalSlider(sliderRect, p_SmoothIntensityCustom2y.floatValue, 0f, 1f);
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_SmoothIntensityCustom2y.name); p_SmoothIntensityCustom2y.floatValue = Mathf.Clamp01(siSVal); }
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_SmoothIntensityCustom2y.hasMixedValue;
                    float siFVal = EditorGUI.FloatField(fieldRect, p_SmoothIntensityCustom2y.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_SmoothIntensityCustom2y.name); p_SmoothIntensityCustom2y.floatValue = Mathf.Clamp01(siFVal); }
                    EditorGUI.BeginDisabledGroup(!cdOn);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_SmoothIntensityCoord.hasMixedValue;
                    if (CDConflict(siCi)) GUI.contentColor = s_ConflictRed;
                    siCi = EditorGUI.Popup(coordRect, siCi, CoordOptions);
                    GUI.contentColor = Color.white;
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_SmoothIntensityCoord.name); p_SmoothIntensityCoord.floatValue = siCi; }
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel = savedI;
                }
                GUILayout.Space(4);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                {
                    var full = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.numberField);
                    float coordW = (full.width - EditorGUIUtility.labelWidth) / 3f;
                    var labelRect = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    var coordRect = new Rect(full.xMax - coordW, full.y, coordW, full.height);
                    var floatRow  = new Rect(full.x, full.y, full.width - coordW, full.height);
                    int erCi = Mathf.Clamp((int)p_DissolveEdgeRangeCoord.floatValue, 0, CoordOptions.Length - 1);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_DissolveEdgeRange.hasMixedValue;
                    float erVal = EditorGUI.FloatField(floatRow, new GUIContent("Edge Width"), p_DissolveEdgeRange.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_DissolveEdgeRange.name); p_DissolveEdgeRange.floatValue = erVal; }
                    if (CDConflict(erCi)) DrawCDConflictLabel(labelRect, "Edge Width", erCi, cdOn);
                    int savedI2 = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginDisabledGroup(!cdOn);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_DissolveEdgeRangeCoord.hasMixedValue;
                    if (CDConflict(erCi)) GUI.contentColor = s_ConflictRed;
                    erCi = EditorGUI.Popup(coordRect, erCi, CoordOptions);
                    GUI.contentColor = Color.white;
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_DissolveEdgeRangeCoord.name); p_DissolveEdgeRangeCoord.floatValue = erCi; }
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel = savedI2;
                }
                m_Editor.ShaderProperty(p_DissolveBlendType, "Edge Blend Mode");
                // Select (= 3) keeps only the edge band without applying color; Edge Color has no effect, so hide it.
                if ((int)p_DissolveBlendType.floatValue != 3)
                    m_Editor.ShaderProperty(p_DissolveEdgeColor, "Edge Color");
                EditorGUILayout.EndVertical();
                GUILayout.Space(4);
                DrawToggleLeft(m_Editor, p_DissolveMaskOn, "Enable Dissolve Mask");
                if (mat.GetFloat("_DissolveMaskOn") == 1f)
                {
                    GUILayout.Space(4);
                    DrawTextureBlock(m_Editor, p_DissolveMask, "Dissolve Mask", p_DissMaskUVMode);
                    DrawWrapModeRow(m_Editor, p_DissMaskWrapMode, p_DissMaskWrapModeU, p_DissMaskWrapModeV, p_DissMaskWrapOn);
                    {
                        var row = EditorGUILayout.GetControlRect();
                        var cont = EditorGUI.PrefixLabel(row, new GUIContent("Texture Channel"));
                        int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                        int ch = Mathf.Clamp((int)p_DissolveMaskChannel.floatValue, 0, 3);
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.showMixedValue = p_DissolveMaskChannel.hasMixedValue;
                        ch = EditorGUI.Popup(cont, ch, new[] { "R", "G", "B", "A" });
                        EditorGUI.showMixedValue = false;
                        if (EditorGUI.EndChangeCheck())
                        { m_Editor.RegisterPropertyChangeUndo(p_DissolveMaskChannel.name); p_DissolveMaskChannel.floatValue = ch; }
                        EditorGUI.indentLevel = savedI;
                    }
                    DrawTexUVOptionsBox(m_Editor, cdOn, p_DissMaskOffsetXCoord, p_DissMaskOffsetYCoord,
                        p_DissolveMaskSpeed, p_DissMaskAngle, p_DissMaskAngleCoord, p_DissMaskAngleSpeed,
                        p_DissMaskOffsetSpeedOn, p_DissMaskAngleSpeedOn);
                    {
                        var full = EditorGUILayout.GetControlRect();
                        float coordW = (full.width - EditorGUIUtility.labelWidth) / 3f;
                        var coordRect = new Rect(full.xMax - coordW, full.y, coordW, full.height);
                        var fieldRect = new Rect(coordRect.xMin - 35f, full.y, 35f, full.height);
                        var sliderRect = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fieldRect.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                        var labelRect = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                        int ci = Mathf.Clamp((int)p_DissolveMaskIntensityCoord.floatValue, 0, CoordOptions.Length - 1);
                        DrawCDConflictLabel(labelRect, "Mask Intensity", ci, cdOn);
                        int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                        EditorGUI.BeginChangeCheck();
                        float sVal = GUI.HorizontalSlider(sliderRect, p_DissolveMaskIntensity.floatValue, 0f, 1f);
                        if (EditorGUI.EndChangeCheck())
                        { m_Editor.RegisterPropertyChangeUndo(p_DissolveMaskIntensity.name); p_DissolveMaskIntensity.floatValue = Mathf.Clamp01(sVal); }
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.showMixedValue = p_DissolveMaskIntensity.hasMixedValue;
                        float fVal = EditorGUI.FloatField(fieldRect, p_DissolveMaskIntensity.floatValue);
                        EditorGUI.showMixedValue = false;
                        if (EditorGUI.EndChangeCheck())
                        { m_Editor.RegisterPropertyChangeUndo(p_DissolveMaskIntensity.name); p_DissolveMaskIntensity.floatValue = Mathf.Clamp01(fVal); }
                        EditorGUI.BeginDisabledGroup(!cdOn);
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.showMixedValue = p_DissolveMaskIntensityCoord.hasMixedValue;
                        if (CDConflict(ci)) GUI.contentColor = s_ConflictRed;
                        ci = EditorGUI.Popup(coordRect, ci, CoordOptions);
                        GUI.contentColor = Color.white;
                        EditorGUI.showMixedValue = false;
                        if (EditorGUI.EndChangeCheck())
                        { m_Editor.RegisterPropertyChangeUndo(p_DissolveMaskIntensityCoord.name); p_DissolveMaskIntensityCoord.floatValue = ci; }
                        EditorGUI.EndDisabledGroup();
                        EditorGUI.indentLevel = savedI;
                    }
                }
            }
            GUILayout.Space(4);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();

        // ── Noise ───────────────────────────────────────────────────────────
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _NoiseFoldout = Foldout(_NoiseFoldout, "Distortion", mat.GetFloat("_NoiseTexOn") == 1f);
        if (_NoiseFoldout)
        {
            EditorGUI.indentLevel++;
            GUILayout.Space(4);
            EditorGUI.BeginChangeCheck();
            DrawToggleLeft(m_Editor, p_NoiseTexOn, "Enable");
            if (EditorGUI.EndChangeCheck() && p_NoiseTexOn.floatValue == 0f)
                foreach (var t in p_NoiseTexOn.targets)
                    DisableKeywords(t as Material, "_USEUVMASK_ON", "_NOISEFLOWMODE_FLOWMAP");
            if (mat.GetFloat("_NoiseTexOn") == 1f)
            {
                GUILayout.Space(4);
                // ─ Distortion Tex header row + Flow Mode dropdown ─
                bool isFlowMap = mat.IsKeywordEnabled("_NOISEFLOWMODE_FLOWMAP");
                {
                    var hdr = EditorGUILayout.GetControlRect();
                    // LabelField calls IndentedRect internally, so pass hdr directly.
                    EditorGUI.LabelField(hdr, "Distortion Tex", EditorStyles.boldLabel);
                    // Align the dropdown with the left edge of the content on the right of the parameter rows below (= hdr.x + labelWidth).
                    float dropX = hdr.x + EditorGUIUtility.labelWidth;
                    int savedI2 = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_NoiseFlowMode.hasMixedValue;
                    int newFlowMode = EditorGUI.Popup(
                        new Rect(dropX, hdr.y, hdr.xMax - dropX, hdr.height),
                        isFlowMap ? 1 : 0, new[] { "Single Channel", "Flow Map" });
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    {
                        bool toFlow = newFlowMode == 1;
                        m_Editor.RegisterPropertyChangeUndo(p_NoiseFlowMode.name);
                        p_NoiseFlowMode.floatValue = toFlow ? 1f : 0f;
                        foreach (var t2 in p_NoiseFlowMode.targets)
                        {
                            var m2 = t2 as Material;
                            if (toFlow) m2.EnableKeyword("_NOISEFLOWMODE_FLOWMAP");
                            else        m2.DisableKeyword("_NOISEFLOWMODE_FLOWMAP");
                            MaterialEditor.ApplyMaterialPropertyDrawers(m2);
                        }
                        isFlowMap = toFlow;
                    }
                    EditorGUI.indentLevel = savedI2;
                }
                DrawTextureBlock(m_Editor, p_NoiseTex, "Distortion Tex", p_NoiseUVMode, false);
                DrawWrapModeRow(m_Editor, p_NoiseWrapMode, p_NoiseWrapModeU, p_NoiseWrapModeV, p_NoiseWrapOn);
                if (isFlowMap)
                {
                    var row = EditorGUILayout.GetControlRect();
                    var cont = EditorGUI.PrefixLabel(row, new GUIContent("Channels"));
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    const float uLblW = 14f;
                    float halfW = (cont.width - 4f) * 0.5f;
                    int chU = Mathf.Clamp((int)p_NoiseFlowChannelU.floatValue, 0, 3);
                    EditorGUI.LabelField(new Rect(cont.x, cont.y, uLblW, cont.height), "U");
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_NoiseFlowChannelU.hasMixedValue;
                    chU = EditorGUI.Popup(new Rect(cont.x + uLblW, cont.y, halfW - uLblW, cont.height), chU, new[] { "R", "G", "B", "A" });
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_NoiseFlowChannelU.name); p_NoiseFlowChannelU.floatValue = chU; }
                    float vX = cont.x + halfW + 4f;
                    int chV = Mathf.Clamp((int)p_NoiseFlowChannelV.floatValue, 0, 3);
                    EditorGUI.LabelField(new Rect(vX, cont.y, uLblW, cont.height), "V");
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_NoiseFlowChannelV.hasMixedValue;
                    chV = EditorGUI.Popup(new Rect(vX + uLblW, cont.y, halfW - uLblW, cont.height), chV, new[] { "R", "G", "B", "A" });
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_NoiseFlowChannelV.name); p_NoiseFlowChannelV.floatValue = chV; }
                    EditorGUI.indentLevel = savedI;
                }
                else
                {
                    var row = EditorGUILayout.GetControlRect();
                    var cont = EditorGUI.PrefixLabel(row, new GUIContent("Texture Channel"));
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    int ch = Mathf.Clamp((int)p_NoiseTexChannel.floatValue, 0, 3);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_NoiseTexChannel.hasMixedValue;
                    ch = EditorGUI.Popup(cont, ch, new[] { "R", "G", "B", "A" });
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_NoiseTexChannel.name); p_NoiseTexChannel.floatValue = ch; }
                    EditorGUI.indentLevel = savedI;
                }
                DrawTexUVOptionsBox(m_Editor, cdOn, p_NoiseOffsetXCoord, p_NoiseOffsetYCoord,
                    p_NoiseSpeed, p_NoiseAngle, p_NoiseAngleCoord, p_NoiseAngleSpeed,
                    p_NoiseOffsetSpeedOn, p_NoiseAngleSpeedOn);
                {
                    var full = EditorGUILayout.GetControlRect();
                    float coordW = (full.width - EditorGUIUtility.labelWidth) / 3f;
                    var labelRect = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    var coordRect = new Rect(full.xMax - coordW, full.y, coordW, full.height);
                    var floatRow = new Rect(full.x, full.y, full.width - coordW, full.height);
                    int ci = Mathf.Clamp((int)p_NoiseIntensityCoord.floatValue, 0, CoordOptions.Length - 1);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_NoiseIntensityCustom2z.hasMixedValue;
                    float nVal = EditorGUI.FloatField(floatRow, new GUIContent("Intensity"), p_NoiseIntensityCustom2z.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_NoiseIntensityCustom2z.name); p_NoiseIntensityCustom2z.floatValue = nVal; }
                    if (CDConflict(ci)) DrawCDConflictLabel(labelRect, "Intensity", ci, cdOn);
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginDisabledGroup(!cdOn);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_NoiseIntensityCoord.hasMixedValue;
                    if (CDConflict(ci)) GUI.contentColor = s_ConflictRed;
                    ci = EditorGUI.Popup(coordRect, ci, CoordOptions);
                    GUI.contentColor = Color.white;
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_NoiseIntensityCoord.name); p_NoiseIntensityCoord.floatValue = ci; }
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel = savedI;
                }
                if (!isFlowMap)
                {
                    m_Editor.ShaderProperty(p_NoiseOffset, "Mid Point");
                    m_Editor.ShaderProperty(p_NoiseDirection, "Direction");
                }
                GUILayout.Space(4);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                DrawToggleLeft(m_Editor, p_UseUVMask, "Enable Distortion Mask");
                if (mat.GetFloat("_UseUVMask") == 1f)
                {
                    GUILayout.Space(4);
                    DrawTextureBlock(m_Editor, p_NoiseUVMask, "Distortion Mask", p_NoiseUVMaskUVMode);
                    DrawWrapModeRow(m_Editor, p_NoiseUVMaskWrapMode, p_NoiseUVMaskWrapModeU, p_NoiseUVMaskWrapModeV, p_NoiseUVMaskWrapOn);
                    {
                        var row = EditorGUILayout.GetControlRect();
                        var cont = EditorGUI.PrefixLabel(row, new GUIContent("Texture Channel"));
                        int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                        int ch = Mathf.Clamp((int)p_NoiseUVMaskChannel.floatValue, 0, 3);
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.showMixedValue = p_NoiseUVMaskChannel.hasMixedValue;
                        ch = EditorGUI.Popup(cont, ch, new[] { "R", "G", "B", "A" });
                        EditorGUI.showMixedValue = false;
                        if (EditorGUI.EndChangeCheck())
                        { m_Editor.RegisterPropertyChangeUndo(p_NoiseUVMaskChannel.name); p_NoiseUVMaskChannel.floatValue = ch; }
                        EditorGUI.indentLevel = savedI;
                    }
                    DrawTexUVOptionsBox(m_Editor, cdOn, p_NoiseUVMaskOffsetXCoord, p_NoiseUVMaskOffsetYCoord,
                        p_NoiseUVMaskSpeed, p_NoiseUVMaskAngle, p_NoiseUVMaskAngleCoord, p_NoiseUVMaskAngleSpeed,
                        p_NoiseUVMaskOffsetSpeedOn, p_NoiseUVMaskAngleSpeedOn);
                    {
                        var full = EditorGUILayout.GetControlRect();
                        float coordW = (full.width - EditorGUIUtility.labelWidth) / 3f;
                        var coordRect = new Rect(full.xMax - coordW, full.y, coordW, full.height);
                        var fieldRect = new Rect(coordRect.xMin - 35f, full.y, 35f, full.height);
                        var sliderRect = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fieldRect.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                        var labelRect = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                        int ci = Mathf.Clamp((int)p_NoiseUVMaskIntensityCoord.floatValue, 0, CoordOptions.Length - 1);
                        DrawCDConflictLabel(labelRect, "Intensity", ci, cdOn);
                        int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                        EditorGUI.BeginChangeCheck();
                        float sVal = GUI.HorizontalSlider(sliderRect, p_NoiseUVMaskIntensity.floatValue, 0f, 1f);
                        if (EditorGUI.EndChangeCheck())
                        { m_Editor.RegisterPropertyChangeUndo(p_NoiseUVMaskIntensity.name); p_NoiseUVMaskIntensity.floatValue = Mathf.Clamp01(sVal); }
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.showMixedValue = p_NoiseUVMaskIntensity.hasMixedValue;
                        float fVal = EditorGUI.FloatField(fieldRect, p_NoiseUVMaskIntensity.floatValue);
                        EditorGUI.showMixedValue = false;
                        if (EditorGUI.EndChangeCheck())
                        { m_Editor.RegisterPropertyChangeUndo(p_NoiseUVMaskIntensity.name); p_NoiseUVMaskIntensity.floatValue = Mathf.Clamp01(fVal); }
                        EditorGUI.BeginDisabledGroup(!cdOn);
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.showMixedValue = p_NoiseUVMaskIntensityCoord.hasMixedValue;
                        if (CDConflict(ci)) GUI.contentColor = s_ConflictRed;
                        ci = EditorGUI.Popup(coordRect, ci, CoordOptions);
                        GUI.contentColor = Color.white;
                        EditorGUI.showMixedValue = false;
                        if (EditorGUI.EndChangeCheck())
                        { m_Editor.RegisterPropertyChangeUndo(p_NoiseUVMaskIntensityCoord.name); p_NoiseUVMaskIntensityCoord.floatValue = ci; }
                        EditorGUI.EndDisabledGroup();
                        EditorGUI.indentLevel = savedI;
                    }
                }
                EditorGUILayout.EndVertical();
                GUILayout.Space(4);
                {
                    var row = EditorGUILayout.GetControlRect();
                    var lbl = new Rect(row.x, row.y, EditorGUIUtility.labelWidth, row.height);
                    var fld = new Rect(lbl.xMax, row.y, row.width - EditorGUIUtility.labelWidth, row.height);
                    EditorGUI.LabelField(lbl, "Targets", EditorStyles.boldLabel);
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_NoiseTargets.hasMixedValue;
                    // Keep only the seven targets supported by the shader; preserve -1 = All unchanged.
                    int ntRaw = (int)p_NoiseTargets.floatValue;
                    var nt = (NoiseTarget)(ntRaw == -1 ? -1 : ntRaw & 127);
                    nt = (NoiseTarget)(int)(object)EditorGUI.EnumFlagsField(fld, nt);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_NoiseTargets.name); p_NoiseTargets.floatValue = (float)(int)nt; }
                    EditorGUI.indentLevel = savedI;
                }
            }
            GUILayout.Space(4);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();

        // ── Vertex Offset ───────────────────────────────────────────────────
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        _VertexFoldout = Foldout(_VertexFoldout, "Vertex Offset", mat.GetFloat("_VertexOffsetOn") == 1f);
        if (_VertexFoldout)
        {
            EditorGUI.indentLevel++;
            GUILayout.Space(4);
            EditorGUI.BeginChangeCheck();
            DrawToggleLeft(m_Editor, p_VertexOffsetOn, "Enable");
            if (EditorGUI.EndChangeCheck() && p_VertexOffsetOn.floatValue == 0f)
                foreach (var t in p_VertexOffsetOn.targets)
                    DisableKeywords(t as Material, "_USEOFFSETMASK_ON", "_OFFSETDIRMODE_NORMAL", "_OFFSETDIRMODE_WORLD","_AXISSCALEMODE_WORLD", "_AXISSCALEMODE_LOCAL");
            if (mat.GetFloat("_VertexOffsetOn") == 1f)
            {
                GUILayout.Space(4);
                DrawTextureBlock(m_Editor, p_VertexOffsetTex, "Offset Tex", p_VtxOffsetUVMode);
                DrawWrapModeRow(m_Editor, p_VtxOffsetWrapMode, p_VtxOffsetWrapModeU, p_VtxOffsetWrapModeV, p_VtxOffsetWrapOn);
                {
                    var row = EditorGUILayout.GetControlRect();
                    var cont = EditorGUI.PrefixLabel(row, new GUIContent("Texture Channel"));
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    int ch = Mathf.Clamp((int)p_VertexOffsetTexChannel.floatValue, 0, 3);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_VertexOffsetTexChannel.hasMixedValue;
                    ch = EditorGUI.Popup(cont, ch, new[] { "R", "G", "B", "A" });
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_VertexOffsetTexChannel.name); p_VertexOffsetTexChannel.floatValue = ch; }
                    EditorGUI.indentLevel = savedI;
                }
                DrawTexUVOptionsBox(m_Editor, cdOn, p_VtxOffsetTexXCoord, p_VtxOffsetTexYCoord,
                    p_OffsetSpeed, p_OffsetTexAngle, p_OffsetTexAngleCoord, p_OffsetTexAngleSpeed,
                    p_VtxTexOffsetSpeedOn, p_VtxTexAngleSpeedOn);
                {
                    var row = EditorGUILayout.GetControlRect();
                    var cont = EditorGUI.PrefixLabel(row, new GUIContent("Direction"));
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    int dirMode = Mathf.Clamp(Mathf.RoundToInt(p_OffsetDirMode.floatValue), 0, 1);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_OffsetDirMode.hasMixedValue;
                    dirMode = EditorGUI.Popup(cont, dirMode, new[] { "Normal", "World" });
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    {
                        m_Editor.RegisterPropertyChangeUndo(p_OffsetDirMode.name);
                        p_OffsetDirMode.floatValue = dirMode;
                        foreach (var t in p_OffsetDirMode.targets)
                            MaterialEditor.ApplyMaterialPropertyDrawers(t as Material);
                    }
                    EditorGUI.indentLevel = savedI;
                }
                m_Editor.ShaderProperty(p_VertexOffsetBaseValue, "Mid Point");
                {
                    var full = EditorGUILayout.GetControlRect();
                    float coordW = (full.width - EditorGUIUtility.labelWidth) / 3f;
                    var labelRect = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    var coordRect = new Rect(full.xMax - coordW, full.y, coordW, full.height);
                    var floatRow = new Rect(full.x, full.y, full.width - coordW, full.height);
                    int iCi = Mathf.Clamp((int)p_VtxIntensityCoord.floatValue, 0, CoordOptions.Length - 1);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_VertexOffsetIntensityCustom2w.hasMixedValue;
                    float iVal = EditorGUI.FloatField(floatRow, new GUIContent("Offset Intensity"), p_VertexOffsetIntensityCustom2w.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_VertexOffsetIntensityCustom2w.name); p_VertexOffsetIntensityCustom2w.floatValue = iVal; }
                    if (CDConflict(iCi)) DrawCDConflictLabel(labelRect, "Intensity", iCi, cdOn);
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginDisabledGroup(!cdOn);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_VtxIntensityCoord.hasMixedValue;
                    if (CDConflict(iCi)) GUI.contentColor = s_ConflictRed;
                    iCi = EditorGUI.Popup(coordRect, iCi, CoordOptions);
                    GUI.contentColor = Color.white;
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_VtxIntensityCoord.name); p_VtxIntensityCoord.floatValue = iCi; }
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel = savedI;
                }
                {
                    var row = EditorGUILayout.GetControlRect();
                    var cont = EditorGUI.PrefixLabel(row, new GUIContent("Axis Scale"));
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    Vector4 dir = p_OffsetDirectionXYZ.vectorValue;
                    float[] vals = { dir.x, dir.y, dir.z };
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_OffsetDirectionXYZ.hasMixedValue;
                    EditorGUI.MultiFloatField(cont, new[] { new GUIContent("X"), new GUIContent("Y"), new GUIContent("Z") }, vals);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_OffsetDirectionXYZ.name); p_OffsetDirectionXYZ.vectorValue = new Vector4(vals[0], vals[1], vals[2], 0f); }
                    EditorGUI.indentLevel = savedI;
                }
                // Scale Mode matters only for Direction = Normal (for World, Axis Scale itself is the direction).
                if (Mathf.RoundToInt(p_OffsetDirMode.floatValue) == 0)
                {
                    var row = EditorGUILayout.GetControlRect();
                    var cont = EditorGUI.PrefixLabel(row, new GUIContent("Scale Mode"));
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    int scaleMode = Mathf.Clamp(Mathf.RoundToInt(p_AxisScaleMode.floatValue), 0, 1);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_AxisScaleMode.hasMixedValue;
                    scaleMode = EditorGUI.Popup(cont, scaleMode, new[] { "World", "Local" });
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    {
                        m_Editor.RegisterPropertyChangeUndo(p_AxisScaleMode.name);
                        p_AxisScaleMode.floatValue = scaleMode;
                        foreach (var t in p_AxisScaleMode.targets)
                            MaterialEditor.ApplyMaterialPropertyDrawers(t as Material);
                    }
                    EditorGUI.indentLevel = savedI;
                }
                GUILayout.Space(4);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                DrawToggleLeft(m_Editor, p_UseOffsetMask, "Enable Offset Mask");
                if (mat.GetFloat("_UseOffsetMask") == 1f)
                {
                    GUILayout.Space(4);
                    DrawTextureBlock(m_Editor, p_VertexOffsetMaskTex, "Offset Mask", p_VtxOffsetMaskUVMode);
                    DrawWrapModeRow(m_Editor, p_VtxMaskWrapMode, p_VtxMaskWrapModeU, p_VtxMaskWrapModeV, p_VtxMaskWrapOn);
                    {
                        var row = EditorGUILayout.GetControlRect();
                        var cont = EditorGUI.PrefixLabel(row, new GUIContent("Texture Channel"));
                        int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                        int ch = Mathf.Clamp((int)p_VtxOffsetMaskChannel.floatValue, 0, 3);
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.showMixedValue = p_VtxOffsetMaskChannel.hasMixedValue;
                        ch = EditorGUI.Popup(cont, ch, new[] { "R", "G", "B", "A" });
                        EditorGUI.showMixedValue = false;
                        if (EditorGUI.EndChangeCheck())
                        { m_Editor.RegisterPropertyChangeUndo(p_VtxOffsetMaskChannel.name); p_VtxOffsetMaskChannel.floatValue = ch; }
                        EditorGUI.indentLevel = savedI;
                    }
                    DrawTexUVOptionsBox(m_Editor, cdOn, p_VtxOffsetMaskXCoord, p_VtxOffsetMaskYCoord,
                        p_VtxOffsetMaskSpeed, p_VtxOffsetMaskAngle, p_VtxOffsetMaskAngleCoord, p_VtxOffsetMaskAngleSpeed,
                        p_VtxMaskOffsetSpeedOn, p_VtxMaskAngleSpeedOn);
                    {
                        var full = EditorGUILayout.GetControlRect();
                        float coordW = (full.width - EditorGUIUtility.labelWidth) / 3f;
                        var coordRect = new Rect(full.xMax - coordW, full.y, coordW, full.height);
                        var fieldRect = new Rect(coordRect.xMin - 35f, full.y, 35f, full.height);
                        var sliderRect = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fieldRect.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                        var labelRect = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                        int ci = Mathf.Clamp((int)p_VtxOffsetMaskIntensityCoord.floatValue, 0, CoordOptions.Length - 1);
                        DrawCDConflictLabel(labelRect, "Mask Intensity", ci, cdOn);
                        int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                        EditorGUI.BeginChangeCheck();
                        float msVal = GUI.HorizontalSlider(sliderRect, p_VtxOffsetMaskIntensity.floatValue, 0f, 1f);
                        if (EditorGUI.EndChangeCheck())
                        { m_Editor.RegisterPropertyChangeUndo(p_VtxOffsetMaskIntensity.name); p_VtxOffsetMaskIntensity.floatValue = Mathf.Clamp01(msVal); }
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.showMixedValue = p_VtxOffsetMaskIntensity.hasMixedValue;
                        float mfVal = EditorGUI.FloatField(fieldRect, p_VtxOffsetMaskIntensity.floatValue);
                        EditorGUI.showMixedValue = false;
                        if (EditorGUI.EndChangeCheck())
                        { m_Editor.RegisterPropertyChangeUndo(p_VtxOffsetMaskIntensity.name); p_VtxOffsetMaskIntensity.floatValue = Mathf.Clamp01(mfVal); }
                        EditorGUI.BeginDisabledGroup(!cdOn);
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.showMixedValue = p_VtxOffsetMaskIntensityCoord.hasMixedValue;
                        if (CDConflict(ci)) GUI.contentColor = s_ConflictRed;
                        ci = EditorGUI.Popup(coordRect, ci, CoordOptions);
                        GUI.contentColor = Color.white;
                        EditorGUI.showMixedValue = false;
                        if (EditorGUI.EndChangeCheck())
                        { m_Editor.RegisterPropertyChangeUndo(p_VtxOffsetMaskIntensityCoord.name); p_VtxOffsetMaskIntensityCoord.floatValue = ci; }
                        EditorGUI.EndDisabledGroup();
                        EditorGUI.indentLevel = savedI;
                    }
                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.Space(4);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();

        // ── Rim ──────────────────────────────────────────────────────────────
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        int rimMode = Mathf.RoundToInt(mat.GetFloat("_RimMode"));
        _RimFoldout = Foldout(_RimFoldout, "Rim", rimMode != 0);
        if (_RimFoldout)
        {
            EditorGUI.indentLevel++;
            GUILayout.Space(4);
            // Rim Mode enum (Off / Fresnel / Matcap): label + narrow popup positioned together at the left.
            {
                var row = EditorGUILayout.GetControlRect();
                int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                const float popupW = 72f;
                EditorGUI.BeginChangeCheck();
                EditorGUI.showMixedValue = p_RimMode.hasMixedValue;
                int newMode = EditorGUI.Popup(new Rect(row.x, row.y, popupW, row.height), rimMode, new[] { "Off", "Fresnel", "Matcap" });
                EditorGUI.showMixedValue = false;
                if (EditorGUI.EndChangeCheck())
                {
                    m_Editor.RegisterPropertyChangeUndo(p_RimMode.name);
                    if (newMode == 2 && rimMode != 2)
                    {
                        p_PreMatCapCullMode.floatValue = p_CullMode.floatValue;
                        p_CullMode.floatValue = 2f;
                    }
                    else if (rimMode == 2 && newMode != 2)
                    {
                        float saved = p_PreMatCapCullMode.floatValue;
                        if (saved >= 0f) p_CullMode.floatValue = saved;
                        p_PreMatCapCullMode.floatValue = -1f;
                    }
                    p_RimMode.floatValue = newMode;
                    foreach (var t in p_RimMode.targets)
                    {
                        var m2 = t as Material;
                        MaterialEditor.ApplyMaterialPropertyDrawers(m2);
                        if (newMode != 1) DisableKeywords(m2, "_FRESNELCHANNEL_RGB", "_FRESNELCHANNEL_A", "_FRESNELBLENDMODE_ADD", "_FRESNELBLENDMODE_MULTIPLY", "_FRESNELBLENDMODE_TINT", "_INVERSE_ON");
                        if (newMode != 2) DisableKeywords(m2, "_MATCAPBLENDMODE_MULTIPLY", "_MATCAPBLENDMODE_ADD", "_MATCAPCOLORMODE_RGB", "_MATCAPCOLORMODE_RGBA", "_MATCAPCOLORMODE_GRAYSCALE", "_MATCAPCOLORMODE_RGBTOA", "_MATCAPNORMALON_ON");
                    }
                    rimMode = newMode;
                }
                EditorGUI.indentLevel = savedI;
            }

            // ── Fresnel params ────────────────────────────────────────────
            if (rimMode == 1)
            {
                GUILayout.Space(4);
                m_Editor.ShaderProperty(p_FresnelChannel, "Output Channel");
                if (!mat.IsKeywordEnabled("_FRESNELCHANNEL_A"))
                {
                    m_Editor.ShaderProperty(p_FresnelColor,     "Fresnel Color");
                    m_Editor.ShaderProperty(p_FresnelBlendMode, "Blend Mode");
                }
                // Progress
                {
                    var full = EditorGUILayout.GetControlRect();
                    float coordW = (full.width - EditorGUIUtility.labelWidth) / 3f;
                    var coordRect = new Rect(full.xMax - coordW, full.y, coordW, full.height);
                    var fieldRect = new Rect(coordRect.xMin - 35f, full.y, 35f, full.height);
                    var sliderRect = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fieldRect.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                    var labelRect = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    int ci = Mathf.Clamp((int)p_FresnelProgressCoord.floatValue, 0, CoordOptions.Length - 1);
                    DrawCDConflictLabel(labelRect, "Progress", ci, cdOn);
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    float sv = GUI.HorizontalSlider(sliderRect, p_FresnelProgress.floatValue, 0f, 1f);
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_FresnelProgress.name); p_FresnelProgress.floatValue = Mathf.Clamp01(sv); }
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_FresnelProgress.hasMixedValue;
                    float fv = EditorGUI.FloatField(fieldRect, p_FresnelProgress.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_FresnelProgress.name); p_FresnelProgress.floatValue = Mathf.Clamp01(fv); }
                    EditorGUI.BeginDisabledGroup(!cdOn);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_FresnelProgressCoord.hasMixedValue;
                    if (CDConflict(ci)) GUI.contentColor = s_ConflictRed;
                    ci = EditorGUI.Popup(coordRect, ci, CoordOptions);
                    GUI.contentColor = Color.white;
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_FresnelProgressCoord.name); p_FresnelProgressCoord.floatValue = ci; }
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel = savedI;
                }
                // Sharpness
                {
                    var full = EditorGUILayout.GetControlRect();
                    float coordW = (full.width - EditorGUIUtility.labelWidth) / 3f;
                    var coordRect = new Rect(full.xMax - coordW, full.y, coordW, full.height);
                    var fieldRect = new Rect(coordRect.xMin - 35f, full.y, 35f, full.height);
                    var sliderRect = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fieldRect.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                    var labelRect = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    int ci = Mathf.Clamp((int)p_FresnelSharpnessCoord.floatValue, 0, CoordOptions.Length - 1);
                    DrawCDConflictLabel(labelRect, "Sharpness", ci, cdOn);
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    float sv = GUI.HorizontalSlider(sliderRect, p_FresnelSharpness.floatValue, 0f, 1f);
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_FresnelSharpness.name); p_FresnelSharpness.floatValue = Mathf.Clamp01(sv); }
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_FresnelSharpness.hasMixedValue;
                    float fv = EditorGUI.FloatField(fieldRect, p_FresnelSharpness.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_FresnelSharpness.name); p_FresnelSharpness.floatValue = Mathf.Clamp01(fv); }
                    EditorGUI.BeginDisabledGroup(!cdOn);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_FresnelSharpnessCoord.hasMixedValue;
                    if (CDConflict(ci)) GUI.contentColor = s_ConflictRed;
                    ci = EditorGUI.Popup(coordRect, ci, CoordOptions);
                    GUI.contentColor = Color.white;
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_FresnelSharpnessCoord.name); p_FresnelSharpnessCoord.floatValue = ci; }
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel = savedI;
                }
                DrawToggleRight(m_Editor, p_UseFresnelAbs, "Enable Fresnel Abs");
                DrawToggleRight(m_Editor, p_Inverse, "Inverse");
                // Intensity
                {
                    var full = EditorGUILayout.GetControlRect();
                    float coordW = (full.width - EditorGUIUtility.labelWidth) / 3f;
                    var coordRect = new Rect(full.xMax - coordW, full.y, coordW, full.height);
                    var fieldRect = new Rect(coordRect.xMin - 35f, full.y, 35f, full.height);
                    var sliderRect = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fieldRect.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                    var labelRect = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    int ci = Mathf.Clamp((int)p_FresnelIntensityCoord.floatValue, 0, CoordOptions.Length - 1);
                    DrawCDConflictLabel(labelRect, "Intensity", ci, cdOn);
                    int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    float sv = GUI.HorizontalSlider(sliderRect, p_FresnelIntensity.floatValue, 0f, 1f);
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_FresnelIntensity.name); p_FresnelIntensity.floatValue = Mathf.Clamp01(sv); }
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_FresnelIntensity.hasMixedValue;
                    float fv = EditorGUI.FloatField(fieldRect, p_FresnelIntensity.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_FresnelIntensity.name); p_FresnelIntensity.floatValue = Mathf.Clamp01(fv); }
                    EditorGUI.BeginDisabledGroup(!cdOn);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_FresnelIntensityCoord.hasMixedValue;
                    if (CDConflict(ci)) GUI.contentColor = s_ConflictRed;
                    ci = EditorGUI.Popup(coordRect, ci, CoordOptions);
                    GUI.contentColor = Color.white;
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_FresnelIntensityCoord.name); p_FresnelIntensityCoord.floatValue = ci; }
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel = savedI;
                }
            }

            // ── Matcap params ─────────────────────────────────────────────
            else if (rimMode == 2)
            {
                GUILayout.Space(4);
                DrawTextureBlock(m_Editor, p_MatCapTex, "Matcap Tex", null);
                DrawWrapModeRow(m_Editor, p_MatCapWrapMode, p_MatCapWrapModeU, p_MatCapWrapModeV, p_MatCapWrapOn);
                GUILayout.Space(4);
                // Angle + AngleSpeed helpBox
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                DrawAngleWithCoord(m_Editor, p_MatCapAngle, p_MatCapAngleCoord, cdOn);
                {
                    const float tW2 = 14f;
                    var row2 = EditorGUILayout.GetControlRect();
                    int si2 = EditorGUI.indentLevel;
                    bool asOn2 = p_MatCapAngleSpeedOn.floatValue > 0.5f;
                    var fr2 = new Rect(row2.x, row2.y, row2.width - (tW2 + 2f), row2.height);
                    EditorGUI.BeginDisabledGroup(!asOn2);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_MatCapAngleSpeed.hasMixedValue;
                    float av2 = EditorGUI.FloatField(fr2, new GUIContent("Angle Speed"), p_MatCapAngleSpeed.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MatCapAngleSpeed.name); p_MatCapAngleSpeed.floatValue = av2; }
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    bool newAsOn2 = EditorGUI.Toggle(new Rect(row2.xMax - tW2, row2.y, tW2, row2.height), asOn2);
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MatCapAngleSpeedOn.name); p_MatCapAngleSpeedOn.floatValue = newAsOn2 ? 1f : 0f; }
                    EditorGUI.indentLevel = si2;
                }
                EditorGUILayout.EndVertical();
                GUILayout.Space(2);

                // Color Mode + Blend Mode
                m_Editor.ShaderProperty(p_MatCapColorMode, "Color Mode");
                m_Editor.ShaderProperty(p_MatCapBlendMode, "Blend Mode");

                // Intensity (slider + CD)
                {
                    var full = EditorGUILayout.GetControlRect();
                    float coordW2 = (full.width - EditorGUIUtility.labelWidth) / 3f;
                    var coordRect2 = new Rect(full.xMax - coordW2, full.y, coordW2, full.height);
                    var fieldRect2 = new Rect(coordRect2.xMin - 35f, full.y, 35f, full.height);
                    var sliderRect2 = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fieldRect2.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                    var labelRect2 = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    int ci2 = Mathf.Clamp((int)p_MatCapIntensityCoord.floatValue, 0, CoordOptions.Length - 1);
                    DrawCDConflictLabel(labelRect2, "Intensity", ci2, cdOn);
                    int si3 = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    float sv2 = GUI.HorizontalSlider(sliderRect2, p_MatCapIntensity.floatValue, 0f, 1f);
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MatCapIntensity.name); p_MatCapIntensity.floatValue = Mathf.Clamp01(sv2); }
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_MatCapIntensity.hasMixedValue;
                    float fv2 = EditorGUI.FloatField(fieldRect2, p_MatCapIntensity.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MatCapIntensity.name); p_MatCapIntensity.floatValue = Mathf.Clamp01(fv2); }
                    EditorGUI.BeginDisabledGroup(!cdOn);
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_MatCapIntensityCoord.hasMixedValue;
                    if (CDConflict(ci2)) GUI.contentColor = s_ConflictRed;
                    ci2 = EditorGUI.Popup(coordRect2, ci2, CoordOptions);
                    GUI.contentColor = Color.white;
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MatCapIntensityCoord.name); p_MatCapIntensityCoord.floatValue = ci2; }
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel = si3;
                }

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                // Emission Scaler (1-50)
                {
                    var full = EditorGUILayout.GetControlRect();
                    var lbl = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    var fld = new Rect(full.xMax - 35f, full.y, 35f, full.height);
                    var sld = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fld.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                    EditorGUI.LabelField(lbl, "Emission Scaler");
                    int si4 = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    float esv = GUI.HorizontalSlider(sld, p_MatCapEmissionScaler.floatValue, 1f, 50f);
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MatCapEmissionScaler.name); p_MatCapEmissionScaler.floatValue = Mathf.Clamp(esv, 1f, 50f); }
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_MatCapEmissionScaler.hasMixedValue;
                    float esf = EditorGUI.FloatField(fld, p_MatCapEmissionScaler.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MatCapEmissionScaler.name); p_MatCapEmissionScaler.floatValue = Mathf.Clamp(esf, 1f, 50f); }
                    EditorGUI.indentLevel = si4;
                }

                // Dim (0-1, Multiply only)
                if (mat.IsKeywordEnabled("_MATCAPBLENDMODE_MULTIPLY"))
                {
                    var full = EditorGUILayout.GetControlRect();
                    var lbl = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    var fld = new Rect(full.xMax - 35f, full.y, 35f, full.height);
                    var sld = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fld.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                    EditorGUI.LabelField(lbl, "Dim");
                    int siD = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    float dv = GUI.HorizontalSlider(sld, p_MatCapDim.floatValue, 0f, 1f);
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MatCapDim.name); p_MatCapDim.floatValue = Mathf.Clamp(dv, 0f, 1f); }
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_MatCapDim.hasMixedValue;
                    float df = EditorGUI.FloatField(fld, p_MatCapDim.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MatCapDim.name); p_MatCapDim.floatValue = Mathf.Clamp(df, 0f, 1f); }
                    EditorGUI.indentLevel = siD;
                }

                // Hue (-180..180, wraps on field input)
                {
                    var full = EditorGUILayout.GetControlRect();
                    var lbl = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    var fld = new Rect(full.xMax - 35f, full.y, 35f, full.height);
                    var sld = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fld.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                    EditorGUI.LabelField(lbl, "Hue");
                    int si5 = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    float hSlider = Mathf.Round(GUI.HorizontalSlider(sld, p_MatCapHue.floatValue, -180f, 180f));
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MatCapHue.name); p_MatCapHue.floatValue = hSlider; }
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_MatCapHue.hasMixedValue;
                    float hField = EditorGUI.FloatField(fld, p_MatCapHue.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    {
                        hField = ((hField % 360f) + 360f) % 360f;
                        if (hField >= 180f) hField -= 360f;
                        m_Editor.RegisterPropertyChangeUndo(p_MatCapHue.name);
                        p_MatCapHue.floatValue = hField;
                    }
                    EditorGUI.indentLevel = si5;
                }

                // Saturation (-1..1)
                {
                    var full = EditorGUILayout.GetControlRect();
                    var lbl = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    var fld = new Rect(full.xMax - 35f, full.y, 35f, full.height);
                    var sld = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fld.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                    EditorGUI.LabelField(lbl, "Saturation");
                    int si6 = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    float satv = GUI.HorizontalSlider(sld, p_MatCapSaturation.floatValue, -1f, 1f);
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MatCapSaturation.name); p_MatCapSaturation.floatValue = Mathf.Clamp(satv, -1f, 1f); }
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_MatCapSaturation.hasMixedValue;
                    float satf = EditorGUI.FloatField(fld, p_MatCapSaturation.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MatCapSaturation.name); p_MatCapSaturation.floatValue = Mathf.Clamp(satf, -1f, 1f); }
                    EditorGUI.indentLevel = si6;
                }

                // Contrast (-1..1)
                {
                    var full = EditorGUILayout.GetControlRect();
                    var lbl = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                    var fld = new Rect(full.xMax - 35f, full.y, 35f, full.height);
                    var sld = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fld.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                    EditorGUI.LabelField(lbl, "Contrast");
                    int si7 = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                    EditorGUI.BeginChangeCheck();
                    float conv = GUI.HorizontalSlider(sld, p_MatCapContrast.floatValue, -1f, 1f);
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MatCapContrast.name); p_MatCapContrast.floatValue = Mathf.Clamp(conv, -1f, 1f); }
                    EditorGUI.BeginChangeCheck();
                    EditorGUI.showMixedValue = p_MatCapContrast.hasMixedValue;
                    float conf = EditorGUI.FloatField(fld, p_MatCapContrast.floatValue);
                    EditorGUI.showMixedValue = false;
                    if (EditorGUI.EndChangeCheck())
                    { m_Editor.RegisterPropertyChangeUndo(p_MatCapContrast.name); p_MatCapContrast.floatValue = Mathf.Clamp(conf, -1f, 1f); }
                    EditorGUI.indentLevel = si7;
                }
                EditorGUILayout.EndVertical();

                // Normal map sub-section
                GUILayout.Space(4);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                DrawToggleLeft(m_Editor, p_MatCapNormalOn, "Enable Normal Map");
                if (mat.GetFloat("_MatCapNormalOn") > 0.5f)
                {
                    GUILayout.Space(4);
                    DrawTextureBlock(m_Editor, p_MatCapNormalTex, "Normal Map", p_MatCapNormalUVMode);
                    DrawWrapModeRow(m_Editor, p_MatCapNormalWrapMode, p_MatCapNormalWrapModeU, p_MatCapNormalWrapModeV, p_MatCapNormalWrapOn);
                    DrawTexUVOptionsBox(m_Editor, cdOn,
                        p_MatCapNormalXCoord, p_MatCapNormalYCoord,
                        p_MatCapNormalSpeed,
                        null, null, null,
                        p_MatCapNormalOffsetSpeedOn, null);
                    // Normal Intensity (0-1)
                    {
                        var full = EditorGUILayout.GetControlRect();
                        var lbl = new Rect(full.x, full.y, EditorGUIUtility.labelWidth, full.height);
                        var fld = new Rect(full.xMax - 35f, full.y, 35f, full.height);
                        var sld = new Rect(full.x + EditorGUIUtility.labelWidth, full.y, fld.xMin - (full.x + EditorGUIUtility.labelWidth) - 4f, full.height);
                        EditorGUI.LabelField(lbl, "Normal Intensity");
                        int si8 = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
                        EditorGUI.BeginChangeCheck();
                        float niv = GUI.HorizontalSlider(sld, p_MatCapNormalIntensity.floatValue, 0f, 1f);
                        if (EditorGUI.EndChangeCheck())
                        { m_Editor.RegisterPropertyChangeUndo(p_MatCapNormalIntensity.name); p_MatCapNormalIntensity.floatValue = Mathf.Clamp(niv, 0f, 1f); }
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.showMixedValue = p_MatCapNormalIntensity.hasMixedValue;
                        float nif = EditorGUI.FloatField(fld, p_MatCapNormalIntensity.floatValue);
                        EditorGUI.showMixedValue = false;
                        if (EditorGUI.EndChangeCheck())
                        { m_Editor.RegisterPropertyChangeUndo(p_MatCapNormalIntensity.name); p_MatCapNormalIntensity.floatValue = Mathf.Clamp(nif, 0f, 1f); }
                        EditorGUI.indentLevel = si8;
                    }
                }
                EditorGUILayout.EndVertical();
            }

            GUILayout.Space(4);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();

        // ── Depth ───────────────────────────────────────────────────────────
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        bool _aotLock = Mathf.RoundToInt(p_AlwaysOnTop.floatValue) == 0;
        string _depthTitle = _aotLock ? "Depth  (* Incompatible with Always On Top)" : "Depth";
        _DepthFoldout = Foldout(_DepthFoldout, _depthTitle, mat.GetFloat("_DepthOn") == 1f);
        if (_DepthFoldout)
        {
            EditorGUI.indentLevel++;
            GUILayout.Space(4);
            EditorGUI.BeginDisabledGroup(_aotLock);
            EditorGUI.BeginChangeCheck();
            DrawToggleLeft(m_Editor, p_DepthOn, "Surface Fade");
            // The _DEPTHBLENDTYPE_* names are keywords from earlier versions of this shader; they are cleared so that
            // materials created back then do not keep them.
            if (EditorGUI.EndChangeCheck() && p_DepthOn.floatValue == 0f)
                foreach (var t in p_DepthOn.targets)
                    DisableKeywords(t as Material, "_DEPTHMETHOD_SIMPLE", "_DEPTHMETHOD_OPACITY_BASED", "_EDGEMODE_ON", "_DEPTHBLENDTYPE_TRANSPARENT", "_DEPTHBLENDTYPE_ALPHABLEND", "_EDGEEFFECTCOLOR_ADD", "_EDGEEFFECTCOLOR_MULTIPLY", "_EDGEEFFECTCOLOR_TINT");
            EditorGUI.EndDisabledGroup();
            if (mat.GetFloat("_DepthOn") == 1f)
            {
                GUILayout.Space(4);
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                m_Editor.ShaderProperty(p_DepthSoftness,  "Depth Softness");
                m_Editor.ShaderProperty(p_DepthMethod,    "Depth Method");
                if (mat.IsKeywordEnabled("_DEPTHMETHOD_OPACITY_BASED"))
                    m_Editor.ShaderProperty(p_DepthOpacityBlend, "  Opacity Factor");
                GUILayout.Space(4);
                DrawToggleLeft(m_Editor, p_EdgeMode, "Edge Mode");
                if (mat.IsKeywordEnabled("_EDGEMODE_ON"))
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    m_Editor.ShaderProperty(p_EdgeColor,       "Edge Color");
                    m_Editor.ShaderProperty(p_EdgeContrast,    "Edge Contrast");
                    m_Editor.ShaderProperty(p_EdgeEffectColor, "Blend Mode");
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
            }
            GUILayout.Space(4);
            DrawToggleLeft(m_Editor, p_CameraFadeOn, "Camera Fade");
            if (mat.IsKeywordEnabled("_CAMERAFADE_ON"))
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                m_Editor.ShaderProperty(p_CameraFadeNear,  "Near");
                m_Editor.ShaderProperty(p_CameraFadeFar,   "Far");
                m_Editor.ShaderProperty(p_CameraFadeWidth, "Width");
                EditorGUILayout.EndVertical();
            }
            GUILayout.Space(4);
            EditorGUI.indentLevel--;
        }
        EditorGUILayout.EndVertical();
    }

    // ────────────────────────────────────────────────────────────────────────
    //  CD conflict helpers
    // ────────────────────────────────────────────────────────────────────────
    // ────────────────────────────────────────────────────────────────────────
    //  Vertex Streams validation
    //
    //  The shader hardcodes the particle data layout. An incorrect stream order silently reads invalid data without reporting an error,
    //  so validate Particle Systems using this material in the scene and provide a one-click fix. There are two cases:
    //
    //  1. Standard rendering (Billboards, etc., or Mesh particles without Mesh GPU Instancing): requirements apply only with Custom Data enabled:
    //       UV + UV2 → TEXCOORD0    Custom1.xyzw → TEXCOORD1    Custom2.xyzw → TEXCOORD2
    //  2. Mesh GPU Instancing (Render Mode = Mesh and Enable Mesh GPU Instancing enabled on the Renderer):
    //     per-particle streams must contain exactly the following entries in this order (matching the shader's instance data struct fields):
    //       Custom Data disabled: Color, AnimFrame
    //       Custom Data enabled: Color, Custom1.xyzw, Custom2.xyzw, AnimFrame
    // ────────────────────────────────────────────────────────────────────────
    readonly List<ParticleSystemRenderer> _psRenderers = new List<ParticleSystemRenderer>();
    readonly List<bool> _psCustomStreamsOn = new List<bool>();   // Whether Custom Vertex Streams is enabled on each Renderer.
    Material _psRenderersMat;
    double   _psRenderersTime = -1.0;

    static readonly List<ParticleSystemVertexStream> s_ActiveStreams = new List<ParticleSystemVertexStream>();

    // Find Particle Systems using this material in the scene (scan at most once per second to avoid traversing the scene on every repaint).
    void RefreshParticleRenderers(Material mat)
    {
        double now = EditorApplication.timeSinceStartup;
        if (_psRenderersMat == mat && now - _psRenderersTime < 1.0) return;
        _psRenderersMat  = mat;
        _psRenderersTime = now;
        _psRenderers.Clear();
        _psCustomStreamsOn.Clear();
        foreach (var r in UnityEngine.Object.FindObjectsByType<ParticleSystemRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (r.sharedMaterial != mat) continue;
            _psRenderers.Add(r);
            // GetActiveVertexStreams returns the last stored list even when Custom Vertex Streams is off, so read the toggle separately.
            var prop = new SerializedObject(r).FindProperty("m_UseCustomVertexStreams");
            _psCustomStreamsOn.Add(prop == null || prop.boolValue);
        }
    }

    // Position / Normal / Color / Tangent have separate semantics; all other streams are packed into TEXCOORD in order.
    static bool UsesTexcoord(ParticleSystemVertexStream s) =>
        s != ParticleSystemVertexStream.Position && s != ParticleSystemVertexStream.Normal &&
        s != ParticleSystemVertexStream.Color    && s != ParticleSystemVertexStream.Tangent;

    // With Mesh GPU Instancing, these streams come from the mesh's own vertex data; all other streams are per-particle streams.
    static bool IsMeshStream(ParticleSystemVertexStream s) =>
        s == ParticleSystemVertexStream.Position || s == ParticleSystemVertexStream.Normal || s == ParticleSystemVertexStream.Tangent ||
        s == ParticleSystemVertexStream.UV  || s == ParticleSystemVertexStream.UV2 ||
        s == ParticleSystemVertexStream.UV3 || s == ParticleSystemVertexStream.UV4;

    static bool IsMeshInstanced(ParticleSystemRenderer r) =>
        r.renderMode == ParticleSystemRenderMode.Mesh && r.enableGPUInstancing && r.supportsMeshInstancing;

    // 1. Required streams for standard rendering with Custom Data enabled.
    static List<ParticleSystemVertexStream> RequiredStreams() => new List<ParticleSystemVertexStream>
    {
        ParticleSystemVertexStream.Position,
        ParticleSystemVertexStream.Normal,
        ParticleSystemVertexStream.Color,
        ParticleSystemVertexStream.UV,            // TEXCOORD0.xy
        ParticleSystemVertexStream.UV2,           // TEXCOORD0.zw
        ParticleSystemVertexStream.Custom1XYZW,   // TEXCOORD1
        ParticleSystemVertexStream.Custom2XYZW,   // TEXCOORD2
    };

    // 2. Required per-particle streams for Mesh GPU Instancing (order matches the shader's instance data struct fields).
    static List<ParticleSystemVertexStream> RequiredInstancedStreams(bool cdOn)
    {
        var req = new List<ParticleSystemVertexStream> { ParticleSystemVertexStream.Color };
        if (cdOn)
        {
            req.Add(ParticleSystemVertexStream.Custom1XYZW);
            req.Add(ParticleSystemVertexStream.Custom2XYZW);
        }
        req.Add(ParticleSystemVertexStream.AnimFrame);
        return req;
    }

    // TEXCOORD streams must come first in exactly this order (additional streams afterward do not affect the layout).
    // Streams with separate semantics only need to be present.
    static bool StreamsMatch(List<ParticleSystemVertexStream> required, List<ParticleSystemVertexStream> active)
    {
        int ai = 0;
        foreach (var r in required)
        {
            if (!UsesTexcoord(r))
            {
                if (!active.Contains(r)) return false;
                continue;
            }
            while (ai < active.Count && !UsesTexcoord(active[ai])) ai++;
            if (ai >= active.Count || active[ai] != r) return false;
            ai++;
        }
        return true;
    }

    // Mesh GPU Instancing: extract per-particle streams; they must match the requirements exactly (no extra or missing entries, and no reordering).
    static bool InstancedStreamsMatch(List<ParticleSystemVertexStream> required, List<ParticleSystemVertexStream> active)
    {
        int ri = 0;
        foreach (var s in active)
        {
            if (IsMeshStream(s)) continue;
            if (ri >= required.Count || required[ri] != s) return false;
            ri++;
        }
        return ri == required.Count;
    }

    void DrawVertexStreamCheck(Material mat, bool cdOn)
    {
        if (_psRenderers.Count == 0) return;

        var required          = RequiredStreams();
        var requiredInstanced = RequiredInstancedStreams(cdOn);
        var bad          = new List<ParticleSystemRenderer>();   // 1. Mismatches in standard rendering.
        var badInstanced = new List<ParticleSystemRenderer>();   // 2. Mismatches in Mesh GPU Instancing.
        for (int i = 0; i < _psRenderers.Count; i++)
        {
            var r = _psRenderers[i];
            if (r == null) continue;
            bool customStreamsOn = _psCustomStreamsOn[i];
            r.GetActiveVertexStreams(s_ActiveStreams);

            if (IsMeshInstanced(r))
            {
                // Custom Vertex Streams off = Unity's default layout (Color, AnimFrame), which matches only when Custom Data is disabled.
                bool ok = customStreamsOn ? InstancedStreamsMatch(requiredInstanced, s_ActiveStreams) : !cdOn;
                if (!ok) badInstanced.Add(r);
            }
            else if (cdOn)
            {
                if (!customStreamsOn || !StreamsMatch(required, s_ActiveStreams)) bad.Add(r);
            }
        }
        if (bad.Count == 0 && badInstanced.Count == 0) return;

        if (bad.Count > 0)
        {
            var names = new List<string>();
            foreach (var s in required) if (UsesTexcoord(s)) names.Add(s.ToString());
            EditorGUILayout.HelpBox(
                $"{bad.Count} Particle System(s) using this material have Vertex Streams that don't match this shader. " +
                "Custom Data will read garbage, with no error shown.\n" +
                "Expected order: " + string.Join(", ", names), MessageType.Warning);
        }
        if (badInstanced.Count > 0)
        {
            var names = new List<string>();
            foreach (var s in requiredInstanced) names.Add(s.ToString());
            EditorGUILayout.HelpBox(
                $"{badInstanced.Count} Particle System(s) using this material render with Mesh GPU Instancing, " +
                "and their per-particle Vertex Streams don't match this shader. Color, Custom Data or animation frames will read garbage.\n" +
                "Expected per-particle streams, in this order and with no others: " + string.Join(", ", names), MessageType.Warning);
        }

        var btn = EditorGUI.IndentedRect(EditorGUILayout.GetControlRect());
        if (GUI.Button(btn, "Fix Vertex Streams"))
        {
            var all = new List<ParticleSystemRenderer>(bad);
            all.AddRange(badInstanced);
            Undo.RecordObjects(all.ToArray(), "Fix Vertex Streams");
            foreach (var r in bad)
            {
                // Place required streams first in order, then append any additional existing streams unchanged.
                r.GetActiveVertexStreams(s_ActiveStreams);
                var fixedStreams = new List<ParticleSystemVertexStream>(required);
                foreach (var s in s_ActiveStreams)
                    if (!fixedStreams.Contains(s)) fixedStreams.Add(s);
                r.SetActiveVertexStreams(fixedStreams);
                EditorUtility.SetDirty(r);
            }
            foreach (var r in badInstanced)
            {
                // Use the mesh's own streams + the required per-particle streams; remove all other per-particle streams (any extra entry misaligns the layout).
                // This list also satisfies case 1, so the same Particle System still works after disabling Mesh GPU Instancing.
                var fixedStreams = new List<ParticleSystemVertexStream>
                {
                    ParticleSystemVertexStream.Position, ParticleSystemVertexStream.Normal, ParticleSystemVertexStream.Color,
                    ParticleSystemVertexStream.UV, ParticleSystemVertexStream.UV2,
                };
                foreach (var s in requiredInstanced)
                    if (!fixedStreams.Contains(s)) fixedStreams.Add(s);
                r.SetActiveVertexStreams(fixedStreams);
                EditorUtility.SetDirty(r);
            }
            _psRenderersTime = -1.0;   // Rescan immediately.
        }
        GUILayout.Space(4);
    }

    // NOTE: BuildCDUsers() (conflict detection) and DrawCustomDataSummary() (the Custom Data Map) each keep their own
    // list of Custom Data bindings. Register a new binding in BOTH, with the same "is this module active" condition.
    static List<string>[] BuildCDUsers(Material mat)
    {
        bool diff2On   = mat.IsKeywordEnabled("_DIFFUSETEX2ON_ON");
        bool dissOn    = mat.IsKeywordEnabled("_DISSOLVEON_ON");
        bool dissMaskOn = dissOn && mat.IsKeywordEnabled("_DISSOLVEMASKON_ON");
        bool maskOn    = mat.IsKeywordEnabled("_MASKTEXON_ON");
        bool noiseOn   = mat.IsKeywordEnabled("_NOISETEXON_ON");
        bool uvMaskOn  = noiseOn && mat.IsKeywordEnabled("_USEUVMASK_ON");
        bool vtxOn     = mat.IsKeywordEnabled("_VERTEXOFFSETON_ON");
        bool vtxMaskOn = vtxOn && mat.IsKeywordEnabled("_USEOFFSETMASK_ON");

        var u = new List<string>[9];
        for (int i = 0; i < 9; i++) u[i] = new List<string>();

        void R(string prop, string axis, string lbl, bool active)
        {
            if (!active) return;
            int ch = Mathf.RoundToInt(mat.GetFloat(prop));
            if (ch >= 1 && ch <= 8) u[ch].Add($"{lbl}.{axis}");
        }

        R("_Diff1OffsetXCoord",           "U",   "Diff1",       true);
        R("_Diff1OffsetYCoord",           "V",   "Diff1",       true);
        R("_Diff1AngleCoord",             "Rot", "Diff1",       true);
        R("_Diff2OffsetXCoord",           "U",   "Diff2",       diff2On);
        R("_Diff2OffsetYCoord",           "V",   "Diff2",       diff2On);
        R("_Diff2AngleCoord",             "Rot", "Diff2",       diff2On);
        R("_Diff2IntensityCoord",         "Int", "Diff2",       diff2On);
        R("_DissTexOffsetXCoord",         "U",   "DissTex",     dissOn);
        R("_DissTexOffsetYCoord",         "V",   "DissTex",     dissOn);
        R("_DissTexAngleCoord",           "Rot", "DissTex",     dissOn);
        R("_DissMaskOffsetXCoord",        "U",   "DissMask",    dissMaskOn);
        R("_DissMaskOffsetYCoord",        "V",   "DissMask",    dissMaskOn);
        R("_DissMaskAngleCoord",          "Rot", "DissMask",    dissMaskOn);
        R("_DissolveMaskIntensityCoord",  "Int", "DissMask",    dissMaskOn);
        R("_MaskOffsetXCoord",            "U",   "Mask",        maskOn);
        R("_MaskOffsetYCoord",            "V",   "Mask",        maskOn);
        R("_MaskAngleCoord",              "Rot", "Mask",        maskOn);
        R("_MaskIntensityCoord",          "Int", "Mask",        maskOn);
        R("_NoiseOffsetXCoord",           "U",   "Noise",       noiseOn);
        R("_NoiseOffsetYCoord",           "V",   "Noise",       noiseOn);
        R("_NoiseAngleCoord",             "Rot", "Noise",       noiseOn);
        R("_NoiseUVMaskOffsetXCoord",     "U",   "NoiseUVMask", uvMaskOn);
        R("_NoiseUVMaskOffsetYCoord",     "V",   "NoiseUVMask", uvMaskOn);
        R("_NoiseUVMaskAngleCoord",       "Rot", "NoiseUVMask", uvMaskOn);
        R("_NoiseUVMaskIntensityCoord",   "Int", "NoiseUVMask", uvMaskOn);
        R("_VtxOffsetTexXCoord",          "U",   "VtxTex",      vtxOn);
        R("_VtxOffsetTexYCoord",          "V",   "VtxTex",      vtxOn);
        R("_OffsetTexAngleCoord",         "Rot", "VtxTex",      vtxOn);
        R("_VtxOffsetMaskXCoord",         "U",   "VtxMask",     vtxMaskOn);
        R("_VtxOffsetMaskYCoord",         "V",   "VtxMask",     vtxMaskOn);
        R("_VtxOffsetMaskAngleCoord",     "Rot", "VtxMask",     vtxMaskOn);
        R("_VtxOffsetMaskIntensityCoord", "Int", "VtxMask",     vtxMaskOn);
        bool fresnelOn = Mathf.RoundToInt(mat.GetFloat("_RimMode")) == 1;
        R("_FresnelProgressCoord",  "Prg", "Fresnel", fresnelOn);
        R("_FresnelSharpnessCoord", "Shp", "Fresnel", fresnelOn);
        R("_FresnelIntensityCoord", "Int", "Fresnel", fresnelOn);
        bool matCapOn = Mathf.RoundToInt(mat.GetFloat("_RimMode")) == 2;
        bool matCapNormalOn = matCapOn && mat.IsKeywordEnabled("_MATCAPNORMALON_ON");
        R("_MatCapAngleCoord",        "Rot", "MatCap",     matCapOn);
        R("_MatCapIntensityCoord",    "Int", "MatCap",     matCapOn);
        R("_MatCapNormalXCoord",      "U",   "MatCapNorm", matCapNormalOn);
        R("_MatCapNormalYCoord",      "V",   "MatCapNorm", matCapNormalOn);
        R("_DissolveRangeCoord",          "Rng", "Diss",        dissOn);
        R("_SmoothIntensityCoord",        "Smt", "Diss",        dissOn);
        R("_DissolveEdgeRangeCoord",      "Edg", "Diss",        dissOn);
        R("_NoiseIntensityCoord",         "Int", "Noise",       noiseOn);
        R("_VtxIntensityCoord",           "Int", "VtxTex",      vtxOn);

        return u;
    }

    static readonly Color s_ConflictRed = new Color(1f, 0.33f, 0.33f);

    bool CDConflict(int ch) =>
        ch >= 1 && ch <= 8 && _cdUsers != null && _cdUsers[ch].Count > 1;

    string CDConflictTip(int ch) =>
        "Channel conflict: " + string.Join(", ", _cdUsers[ch]);

    void DrawCDConflictLabel(Rect labelRect, string text, int ch, bool cdEnabled = true)
    {
        if (CDConflict(ch))
        {
            var indented = EditorGUI.IndentedRect(labelRect);
            int savedI = EditorGUI.indentLevel; EditorGUI.indentLevel = 0;
            // Icon: always at full brightness; unaffected by disabled styling.
            var ic = EditorGUIUtility.IconContent("console.warnicon.sml");
            ic.tooltip = CDConflictTip(ch);
            GUI.Label(new Rect(indented.x - 16f, labelRect.y + 1f, 16f, 16f), ic);
            // Label: use disabled styling when cdEnabled = false; also fix the hover color to red to prevent a color change on hover.
            var red = new GUIStyle(EditorStyles.label)
            {
                normal = { textColor = s_ConflictRed },
                hover  = { textColor = s_ConflictRed },
            };
            if (!cdEnabled) EditorGUI.BeginDisabledGroup(true);
            GUI.Label(indented, text, red);
            if (!cdEnabled) EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel = savedI;
        }
        else
        {
            EditorGUI.LabelField(labelRect, text);
        }
    }

    //  Custom Data Map
    // ────────────────────────────────────────────────────────────────────────
    static void DrawCustomDataSummary(Material mat)
    {
        bool diff2On = mat.IsKeywordEnabled("_DIFFUSETEX2ON_ON");
        bool dissOn = mat.IsKeywordEnabled("_DISSOLVEON_ON");
        bool dissMaskOn = dissOn && mat.IsKeywordEnabled("_DISSOLVEMASKON_ON");
        bool maskOn = mat.IsKeywordEnabled("_MASKTEXON_ON");
        bool noiseOn = mat.IsKeywordEnabled("_NOISETEXON_ON");
        bool uvMaskOn = noiseOn && mat.IsKeywordEnabled("_USEUVMASK_ON");
        bool vtxOn = mat.IsKeywordEnabled("_VERTEXOFFSETON_ON");
        bool vtxMaskOn = vtxOn && mat.IsKeywordEnabled("_USEOFFSETMASK_ON");

        var users    = new System.Collections.Generic.List<string>[9];
        var isCustom = new bool[9];
        for (int i = 0; i < 9; i++) users[i] = new System.Collections.Generic.List<string>();

        void Reg(string propName, string axis, string lbl, bool active, int defaultCh = 0)
        {
            if (!active) return;
            int ch = Mathf.RoundToInt(mat.GetFloat(propName));
            if (ch >= 1 && ch <= 8)
            {
                users[ch].Add($"{lbl}.{axis}");
                if (ch != defaultCh) isCustom[ch] = true;
            }
        }

        Reg("_Diff1OffsetXCoord", "U", "Diff1", true, 1);
        Reg("_Diff1OffsetYCoord", "V", "Diff1", true, 2);
        Reg("_Diff1AngleCoord", "Rot", "Diff1", true);
        Reg("_Diff2OffsetXCoord", "U", "Diff2", diff2On);
        Reg("_Diff2OffsetYCoord", "V", "Diff2", diff2On);
        Reg("_Diff2AngleCoord", "Rot", "Diff2", diff2On);
        Reg("_Diff2IntensityCoord", "Int", "Diff2", diff2On);
        Reg("_DissTexOffsetXCoord", "U", "DissTex", dissOn);
        Reg("_DissTexOffsetYCoord", "V", "DissTex", dissOn);
        Reg("_DissTexAngleCoord", "Rot", "DissTex", dissOn);
        Reg("_DissMaskOffsetXCoord", "U", "DissMask", dissMaskOn);
        Reg("_DissMaskOffsetYCoord", "V", "DissMask", dissMaskOn);
        Reg("_DissMaskAngleCoord", "Rot", "DissMask", dissMaskOn);
        Reg("_DissolveMaskIntensityCoord", "Int", "DissMask", dissMaskOn);
        Reg("_MaskOffsetXCoord", "U", "Mask", maskOn, 3);
        Reg("_MaskOffsetYCoord", "V", "Mask", maskOn, 4);
        Reg("_MaskAngleCoord", "Rot", "Mask", maskOn);
        Reg("_MaskIntensityCoord", "Int", "Mask", maskOn);
        Reg("_NoiseOffsetXCoord", "U", "Noise", noiseOn);
        Reg("_NoiseOffsetYCoord", "V", "Noise", noiseOn);
        Reg("_NoiseAngleCoord", "Rot", "Noise", noiseOn);
        Reg("_NoiseUVMaskOffsetXCoord", "U", "NoiseUVMask", uvMaskOn);
        Reg("_NoiseUVMaskOffsetYCoord", "V", "NoiseUVMask", uvMaskOn);
        Reg("_NoiseUVMaskAngleCoord",     "Rot", "NoiseUVMask", uvMaskOn);
        Reg("_NoiseUVMaskIntensityCoord", "Int", "NoiseUVMask", uvMaskOn);
        Reg("_VtxOffsetTexXCoord", "U", "VtxTex", vtxOn);
        Reg("_VtxOffsetTexYCoord", "V", "VtxTex", vtxOn);
        Reg("_OffsetTexAngleCoord", "Rot", "VtxTex", vtxOn);
        Reg("_VtxOffsetMaskXCoord", "U", "VtxMask", vtxMaskOn);
        Reg("_VtxOffsetMaskYCoord", "V", "VtxMask", vtxMaskOn);
        Reg("_VtxOffsetMaskAngleCoord", "Rot", "VtxMask", vtxMaskOn);
        Reg("_VtxOffsetMaskIntensityCoord", "Int", "VtxMask", vtxMaskOn);
        bool fresnelOn2  = Mathf.RoundToInt(mat.GetFloat("_RimMode")) == 1;
        Reg("_FresnelProgressCoord",   "Prg", "Fresnel",    fresnelOn2);
        Reg("_FresnelSharpnessCoord",  "Shp", "Fresnel",    fresnelOn2);
        Reg("_FresnelIntensityCoord",  "Int", "Fresnel",    fresnelOn2);
        bool matCapOn2   = Mathf.RoundToInt(mat.GetFloat("_RimMode")) == 2;
        bool mcNormalOn2 = matCapOn2 && mat.IsKeywordEnabled("_MATCAPNORMALON_ON");
        Reg("_MatCapAngleCoord",       "Rot", "MatCap",     matCapOn2);
        Reg("_MatCapIntensityCoord",   "Int", "MatCap",     matCapOn2);
        Reg("_MatCapNormalXCoord",     "U",   "MatCapNorm", mcNormalOn2);
        Reg("_MatCapNormalYCoord",     "V",   "MatCapNorm", mcNormalOn2);

        Reg("_DissolveRangeCoord", "Rng", "Diss", dissOn, 5);
        Reg("_SmoothIntensityCoord", "Smt", "Diss", dissOn, 6);
        Reg("_DissolveEdgeRangeCoord", "Edg", "Diss", dissOn);
        Reg("_NoiseIntensityCoord", "Int", "Noise", noiseOn, 7);
        Reg("_VtxIntensityCoord", "Int", "VtxTex", vtxOn, 8);

        string[] chNames = { null, "CD1.x", "CD1.y", "CD1.z", "CD1.w",
                                    "CD2.x", "CD2.y", "CD2.z", "CD2.w" };

        GUIStyle MkCol(Color c) => new(EditorStyles.label) { fontSize = 11, normal = { textColor = c }, hover = { textColor = c } };

        var grayStyle  = MkCol(new Color(0.55f, 0.55f, 0.55f));
        var whiteStyle = MkCol(Color.white);
        var blueStyle  = MkCol(new Color(0.45f, 0.75f, 1.0f));
        var redStyle   = MkCol(new Color(1f, 0.33f, 0.33f));
        var titleStyle = new GUIStyle(EditorStyles.boldLabel) { fontSize = 11 };

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("Custom Data Map", titleStyle);
        GUILayout.Space(2);

        for (int ch = 1; ch <= 8; ch++)
        {
            var list = users[ch];
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(4);
            if (list.Count == 0)
            {
                GUILayout.Label("○", grayStyle, GUILayout.Width(14));
                GUILayout.Label(chNames[ch], grayStyle, GUILayout.Width(42));
                GUILayout.Label("—", grayStyle, GUILayout.Width(18));
                GUILayout.Label("None", grayStyle);
            }
            else if (list.Count > 1)
            {
                GUILayout.Label("⚠", redStyle, GUILayout.Width(14));
                GUILayout.Label(chNames[ch], redStyle, GUILayout.Width(42));
                GUILayout.Space(-2);
                GUILayout.Label("⟶", redStyle, GUILayout.Width(22));
                GUILayout.Label(string.Join(" / ", list), redStyle);
            }
            else
            {
                var s = isCustom[ch] ? blueStyle : whiteStyle;
                GUILayout.Label(isCustom[ch] ? "◆" : "●", s, GUILayout.Width(14));
                GUILayout.Label(chNames[ch], s, GUILayout.Width(42));
                GUILayout.Space(-2);
                GUILayout.Label("⟶", s, GUILayout.Width(22));
                GUILayout.Label(list[0], s);
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }
}
#endif
