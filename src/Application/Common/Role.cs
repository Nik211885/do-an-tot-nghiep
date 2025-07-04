using System.Text.Json.Serialization;

namespace Application.Common;

public enum Role
{
    /// <summary>
    ///  role default when user has register use for system 
    /// </summary>
    User,
    /// <summary>
    ///  Moderator create make moderation context 
    /// </summary>
    Moderator,
    /// <summary>
    /// Management system
    /// </summary>
    Admin   
}
