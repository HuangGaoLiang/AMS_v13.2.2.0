using System.ComponentModel;

namespace AMS.Core
{
    /// <summary>
    /// 返回结果类型
    /// </summary>
    public enum ResultType
    {
        [Description("成功")]
        Success = 1,
        [Description("签名错误")]
        SignError = 2,
        [Description("参数错误")]
        ParaError = 4,
        [Description("添加失败")]
        AddFail = 8,
        [Description("更新失败")]
        UpdateFail = 16,
        [Description("对象不存在")]
        ObjectNull = 32,
        [Description("对象已存在")]
        ObjectExsit = 64,
        [Description("对象状态不正常")]
        ObjectStateError = 128,
        [Description("未知操作")]
        UnKnowOperate = 256,
        [Description("未知来源")]
        UnKnowSource = 512,
        [Description("未登录")]
        UnAuthorize = 1024,
        [Description("权限不足")]
        NoRight = 2048,
        [Description("内部错误")]
        InnerError = 4096
    }
}
