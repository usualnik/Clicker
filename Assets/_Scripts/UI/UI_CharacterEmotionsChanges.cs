using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterEmotionsChanges : MonoBehaviour
{
    [SerializeField] private Sprite[] _emotions;
    private Image _characterImage;
    private int _clicksUntillReset;
    private const int CLICKS_UNTIL_RESET_MAX = 20;
    private bool _shouldReset = false;

    private void Awake()
    {
        _characterImage = GetComponent<Image>();
    }

    private void Start()
    {
        Narrator.Instance.OnReplicaPrinted += Instance_OnReplicaPrinted;
        ClickRegister.Instance.OnClick += Clickregister_OnClick;
    }
  
    private void OnDestroy()
    {
        Narrator.Instance.OnReplicaPrinted -= Instance_OnReplicaPrinted;
        ClickRegister.Instance.OnClick -= Clickregister_OnClick;
    }

    private void Clickregister_OnClick()
    {
       TryResetEmotion();
    }

    private void TryResetEmotion()
    {
        if (_shouldReset)
        {
            _clicksUntillReset -= 1;

            if (_clicksUntillReset <= 0)
            {
                _characterImage.sprite = _emotions[0];
                _shouldReset = false;
            }
        }
    }

    private void Instance_OnReplicaPrinted(string replica)
    {
        CheckShouldChaneEmotion(replica);
    }
    private void CheckShouldChaneEmotion(string replica)
    {
        switch (replica)
        {
            case "Да у тебя талант!":
                _characterImage.sprite = _emotions[4];
                _shouldReset = true;
                _clicksUntillReset = CLICKS_UNTIL_RESET_MAX;
                break;
            case "You've got talent!":
                _characterImage.sprite = _emotions[4];
                _shouldReset = true;
                _clicksUntillReset = CLICKS_UNTIL_RESET_MAX;
                break;
            case "А посмотри-ка ты наверх, слева. Можешь купить что-нибудь.":
                _characterImage.sprite = _emotions[1];
                break;
            case "Take a look up there, on the left. You can buy something.":
                _characterImage.sprite = _emotions[1];
                break;
            case "Наконец, кто-то кто понимает как надо. Продолжай...":
                _characterImage.sprite = _emotions[3];
                break;
            case "Finally, someone who understands how it's done. Keep going...":
                _characterImage.sprite = _emotions[3];
                break;
            case "Отлично справляешься...":
                _characterImage.sprite = _emotions[2];
                break;
            case "You are doing great...":
                _characterImage.sprite = _emotions[2];
                break;
            case "Мм, ловкие у тебя руки...":
                _characterImage.sprite = _emotions[3];
                break;
            case "Mmm, your hands are skillful...":
                _characterImage.sprite = _emotions[3];
                break;
            case "Оу, кисточка? Чтож, удиви меня.":
                _characterImage.sprite = _emotions[3];
                break;
            case "Oh, a brush? Well then, impress me.":
                _characterImage.sprite = _emotions[3];
                break;
            case "Серьёзно, почему ты так хорошо умеешь управляться с кисточкой?":
                _characterImage.sprite = _emotions[2];
                break;
            case "I am not judging you, if anything... Seriously, why are you so good with a brush?":
                _characterImage.sprite = _emotions[2];
                break;
            case "Ошибаюсь? Эх, жаль... Я бы могла посидеть пару часов перед тобой.":
                _characterImage.sprite = _emotions[3];
                break;
            case "Am I mistaken? What a shame... I wouldn't mind sitting in front of you for a couple of hours.":
                _characterImage.sprite = _emotions[3];
                break;
            case "Можно чуть левее?":
                _characterImage.sprite = _emotions[2];
                break;
            case "Could you go a bit more to the left?":
                _characterImage.sprite = _emotions[2];
                break;
            case "Мм... Это коммерческий секрет.":
                _characterImage.sprite = _emotions[3];
                break;
            case "Oh? You are asking where the catch is? So why am I paying you for taps? Mmm... That is a trade secret.":
                _characterImage.sprite = _emotions[3];
                break;
            case "А, поглаживания...":
                _characterImage.sprite = _emotions[3];
                break;
            case "Oh, pats...":
                _characterImage.sprite = _emotions[3];
                break;
            case "А ещё ты мне все волосы распутал.":
                _characterImage.sprite = _emotions[4];
                break;
            case "Aaand you untangled all my hair.":
                _characterImage.sprite = _emotions[4];
                break;
            case "Так, что ты задумал на этот раз? Гигиеническая помада?":
                _characterImage.sprite = _emotions[3];
                break;
            case "So, what are you up to this time? Lip balm?":
                _characterImage.sprite = _emotions[3];
                break;
            case "Эээ... это... надеюсь муж не узнает...":
                _characterImage.sprite = _emotions[5];
                break;
            case "Uhh... this... I hope my husband does not find out...":
                _characterImage.sprite = _emotions[5];
                break;
            case "Нет, ничего, я просто с клиентом никогда этого не делала... Приятно очень даже...":
                _characterImage.sprite = _emotions[6];
                break;
            case "No, it is nothing, I have just never done this with a client... It feels really nice...":
                _characterImage.sprite = _emotions[6];
                break;
            case "Какой настойчивый! Такое мне нравится.":
                _characterImage.sprite = _emotions[7];
                break;
            case "You are so persistent! I like that.":
                _characterImage.sprite = _emotions[7];
                break;
            case "Ты меня в губы что ли поцеловал?":
                _characterImage.sprite = _emotions[8];
                break;
            case "Did you just kiss me on the lips?":
                _characterImage.sprite = _emotions[8];
                break;
        }

        

    }
}
