namespace CORE.APP.Models;
/// Represents the result of executing a command operation (e.g., create, update, delete).
/// Includes unique identifier, success status and a operation result message.
/// Inherits from the base Response abstract class.
public class CommandResponse:Response
{
    /// get; sadece okuma izni verir → dışarıdan değiştirilemez, güvenlidir.
    ///Yani değer sadece constructor içinde set edilir.
    /// Gets a value indicating whether the command was executed successfully.
    public bool IsSuccesful { get; }
    
    /// Gets a message that provides additional information about the command execution.
    /// This may include error details, confirmation messages, or custom status descriptions.
    public string Message { get; }

    public CommandResponse(bool isSuccesful, string message = "", int id = 0) : base(id)
    {
        IsSuccesful = isSuccesful;
        Message = message;
    }
    
}