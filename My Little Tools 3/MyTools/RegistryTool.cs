using Microsoft.Win32;

namespace MyLittleTools3.MyTools
{
    internal class RegistryTool
    {
        private readonly RegistryKey _key = Registry.LocalMachine.CreateSubKey("Software\\MyLittleTools");


        public void SetValue(string name, string value)
        {
            _key.SetValue(name, value, RegistryValueKind.String);
            _key.Close();
        }

        public string GetValue(string name)
        {
            var value = _key.GetValue(name).ToString();
            _key.Close();
            return value;
        }

    }
}
