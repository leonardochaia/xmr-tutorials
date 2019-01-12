using System;

namespace MoneroInteractive.WebApp.AppStatus.DataTransfer
{
    public class AppStatusDto : IEquatable<AppStatusDto>
    {
        public bool HasEnoughBalance { get; set; }

        public bool Equals(AppStatusDto other)
        {
            if (other == null) return false;
            return other.HasEnoughBalance == HasEnoughBalance;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals(obj as AppStatusDto);
        }

        public override int GetHashCode()
        {
            return HasEnoughBalance.GetHashCode();
        }
    }
}