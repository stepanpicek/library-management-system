namespace LibraryManagementSystem.Web.Models;

public enum ToastType
{
    Success,
    Error,
    Info,
    Warning
}

public class ToastMessage
{
    public string Message { get; }
    public ToastType Type { get; }

    public ToastMessage(string message, ToastType type)
    {
        Message = message;
        Type = type;
    }
}