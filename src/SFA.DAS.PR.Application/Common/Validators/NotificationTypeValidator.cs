using FluentValidation;
using SFA.DAS.PR.Domain.Common;

namespace SFA.DAS.PR.Application.Common.Validators
{
    public static class NotificationTypeValidator
    {
        public const string NotificationTypeValidationMessage = "NotificationType must be either Provider or Employer";
        public static IRuleBuilderOptions<T, string> ValidNotificationType<T>(this IRuleBuilderInitial<T, string> ruleBuilder)
        {
            return ruleBuilder
                .Must(a => BeValidNotificationType(a))
                .WithMessage(NotificationTypeValidationMessage);
        }

        private static bool BeValidNotificationType(string notificationType)
        {
            return Enum.TryParse(typeof(NotificationType), notificationType, true, out _);
        }
    }
}
