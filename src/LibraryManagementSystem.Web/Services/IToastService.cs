using LibraryManagementSystem.Web.Models;

namespace LibraryManagementSystem.Web.Services;

public interface IToastService
{
    public event Action<ToastMessage>? OnShow;

    public void Success(string message);

    public void Error(string message);

    public void Info(string message);

    public void Warning(string message);
}