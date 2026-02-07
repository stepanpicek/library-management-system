using LibraryManagementSystem.Web.Models;

namespace LibraryManagementSystem.Web.Services;

public class ToastService : IToastService
{
    public event Action<ToastMessage>? OnShow;

    public void Success(string message)
        => OnShow?.Invoke(new ToastMessage(message, ToastType.Success));

    public void Error(string message)
        => OnShow?.Invoke(new ToastMessage(message, ToastType.Error));

    public void Info(string message)
        => OnShow?.Invoke(new ToastMessage(message, ToastType.Info));

    public void Warning(string message)
        => OnShow?.Invoke(new ToastMessage(message, ToastType.Warning));
}