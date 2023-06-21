namespace ComeCapture.Models
{
    public enum Tool
    {
        /// <summary>
        /// 无状态可移动
        /// </summary>
        Null,
        /// <summary>
        /// 长方形
        /// </summary>
        Rectangle,
        /// <summary>
        /// 圆形
        /// </summary>
        Ellipse,
        /// <summary>
        /// 画刷
        /// </summary>
        Line,
        /// <summary>
        /// 文字
        /// </summary>
        Text,
        /// <summary>
        /// 箭头
        /// </summary>
        Arrow,
        /// <summary>
        /// 开关灯
        /// </summary>
        Light,
        /// <summary>
        /// 最大化截图
        /// </summary>
        MaxEditImage,
        /// <summary>
        /// 撤销
        /// </summary>
        Revoke,
        /// <summary>
        /// 保存
        /// </summary>
        Save,
        /// <summary>
        /// 退出截图
        /// </summary>
        Cancel,
        /// <summary>
        /// 完成截图
        /// </summary>
        OK
    }
}
