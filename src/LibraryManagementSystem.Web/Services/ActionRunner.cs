using FluentValidation;
using LibraryManagementSystem.Domain.Exceptions;

namespace LibraryManagementSystem.Web.Services;

public class ActionRunner(IToastService toastService) : IActionRunner
{
    public async Task RunAsync(
        Func<Task> action, 
        Func<ValidationException, Task>? onValidation = null,
        Func<DomainException, Task>? onDomain = null,
        Func<Exception, Task>? onUnexpected = null)
    {
        try
        {
            await action();
        }
        catch (ValidationException ex)
        {
            if (onValidation is not null)
            {
                await onValidation(ex);
            }
            else
            {
                var msg = string.Join(" ", ex.Errors.Select(e => e.ErrorMessage));
                toastService.Error(msg);
            }
        }
        catch (DomainException ex)
        {
            if (onDomain is not null)
            {
                await onDomain(ex);
            }
            else
            {
                toastService.Error(ex.Message);
            }
        }
        catch (Exception ex)
        {
            if (onUnexpected is not null)
            {
                await onUnexpected(ex);
            }
            else
            {
                toastService.Error("Unexpected error. Please try again.");
                throw; 
            }
        }
    }
}