using FluentValidation;
using LibraryManagementSystem.Domain.Exceptions;

namespace LibraryManagementSystem.Web.Services;

public interface IActionRunner
{
    Task RunAsync(
        Func<Task> action, 
        Func<ValidationException, Task>? onValidation = null,
        Func<DomainException, Task>? onDomain = null,
        Func<Exception, Task>? onUnexpected = null);
}