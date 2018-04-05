using Microsoft.Win32;

namespace MyLittleTools3
{
    class RegistryTool
    {
        RegistryKey key = Registry.LocalMachine.CreateSubKey("Software\\MyLittleTools");


        public void SetValue(string name, string value)
        {
            key.SetValue(name, value, RegistryValueKind.String);
            key.Close();
        }

        public string GetValue(string name)
        {
            string value = key.GetValue(name).ToString();
            key.Close();
            return value;
        }

    }
}
