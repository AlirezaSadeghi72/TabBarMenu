using System;
using System.ComponentModel;

namespace Atiran.Utility.Docking
{
    [AttributeUsage(AttributeTargets.All)]
    internal sealed class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private bool m_initialized;

        public LocalizedDescriptionAttribute(string key) : base(key)
        {
        }

        public override string Description
        {
            get
            {
                if (!m_initialized)
                {
                    var key = base.Description;
                    DescriptionValue = ResourceHelper.GetString(key);
                    if (DescriptionValue == null)
                        DescriptionValue = string.Empty;

                    m_initialized = true;
                }

                return DescriptionValue;
            }
        }
    }

    [AttributeUsage(AttributeTargets.All)]
    internal sealed class LocalizedCategoryAttribute : CategoryAttribute
    {
        public LocalizedCategoryAttribute(string key) : base(key)
        {
        }

        protected override string GetLocalizedString(string key)
        {
            return ResourceHelper.GetString(key);
        }
    }
}