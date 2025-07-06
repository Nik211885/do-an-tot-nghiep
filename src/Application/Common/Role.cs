using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Application.Common;

public enum Role
{
    /// <summary>
    ///  role default when user has register use for system 
    /// </summary>
    [Description("author")]
    Author,
    /// <summary>
    ///  Moderator create make moderation context 
    /// </summary>
    [Description("moderation")]
    Moderation,
    /// <summary>
    /// Management system
    /// </summary>
    [Description("manager-resources")]
    ManagerResources   
}
