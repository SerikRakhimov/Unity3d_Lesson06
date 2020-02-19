using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordPanel : MonoBehaviour
{
    [SerializeField]
    private Transform root;

    [SerializeField]
    private Text pass0_Text, pass1_Text, pass2_Text, pass3_Text, digit0_Text, digit1_Text, digit2_Text, digit3_Text;

    [SerializeField]
    private Button plus0_Btn, minus0_Btn, plus1_Btn, minus1_Btn, plus2_Btn, minus2_Btn, plus3_Btn, minus3_Btn;

    // массив из 4-х цифр вводимого пароля
    private int[] nums = new int[4] { 0, 0, 0, 0 };

    // массив из 4-х цифр правильного пароля
    private int[] pass = new int[4] { 1, 2, 3, 7 };
    
    private void Start()
    {
        // отключаем панель на старте игры
        root.gameObject.SetActive(false);

	// присвоим значения правильного пароля для отображения на экране
	pass0_Text.text = pass[0].ToString();
	pass1_Text.text = pass[1].ToString();
	pass2_Text.text = pass[2].ToString();
	pass3_Text.text = pass[3].ToString();

        // назначаем обработчики нажатий кнопок
        plus0_Btn.onClick.AddListener(() => {OnBtnClick(0,1);});
        minus0_Btn.onClick.AddListener(() => {OnBtnClick(0,-1);});
        plus1_Btn.onClick.AddListener(() => {OnBtnClick(1,1);});
        minus1_Btn.onClick.AddListener(() => {OnBtnClick(1,-1);});
        plus2_Btn.onClick.AddListener(() => {OnBtnClick(2,1);});
        minus2_Btn.onClick.AddListener(() => {OnBtnClick(2,-1);});
        plus3_Btn.onClick.AddListener(() => {OnBtnClick(3,1);});
        minus3_Btn.onClick.AddListener(() => {OnBtnClick(3,-1);});
	SetDigitText();
    }

    public void SetPanelActive(bool state)
    {
        root.gameObject.SetActive(state);
    }

    public void SetDigitText()
    {
	// присвоим значения вводимого пароля для отображения на экране
	digit0_Text.text = nums[0].ToString();
	digit1_Text.text = nums[1].ToString();
	digit2_Text.text = nums[2].ToString();
	digit3_Text.text = nums[3].ToString();
    
    }

    // пароль введен верно
    public bool PasswordCorrect()
    {
        bool result = (nums[0] == pass[0] && nums[1] == pass[1] && nums[2] == pass[2] && nums[3] == pass[3]);
	return result;
    }

    private void OnBtnClick(int btnNum, int value)
    {
	int calc;
	if ((0 <= btnNum) && (btnNum <= 3))
        {
		calc = nums[btnNum];
		calc += value;

		if (calc < 0)
		{
			calc = 9;
		}

		if (calc > 9)
		{
			calc = 0;
		}

		nums[btnNum] = calc;

		SetDigitText();
	}
    }

    private void OnDestroy()
    {
        plus0_Btn.onClick.RemoveAllListeners();
        minus0_Btn.onClick.RemoveAllListeners();
        plus1_Btn.onClick.RemoveAllListeners();
        minus1_Btn.onClick.RemoveAllListeners();
        plus2_Btn.onClick.RemoveAllListeners();
        minus2_Btn.onClick.RemoveAllListeners();
        plus3_Btn.onClick.RemoveAllListeners();
        minus3_Btn.onClick.RemoveAllListeners();
    }  
}
