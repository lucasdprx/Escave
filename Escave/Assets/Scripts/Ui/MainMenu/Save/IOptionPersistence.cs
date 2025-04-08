using UnityEngine;

public interface IOptionPersistence
{
    void LoadOption(OptionsData _optionData);
    void SaveOption(ref OptionsData _optionData);
}
