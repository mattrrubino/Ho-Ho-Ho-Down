using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.IO.Compression;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] GameObject initialInput;
    TMP_InputField nameInput;
    GameManager gm;
    [SerializeField] TextMeshProUGUI heading;
    [SerializeField] TextMeshProUGUI[] topFive;
    [SerializeField] Button submitButton;
    [SerializeField] TextMeshProUGUI initialHeader;
    string[] names = new string[5];
    string[] scores = new string[5];
    string[] words = {"ass", "dik", "dic", "nig", "kkk", "cum", "fuk", "fuc", "coc",
    "cok", "sht", "cnt", "cfm", "jew", "fag", "gay", "tit", "pus", "jiz", "slt", "fuq",
    "fux", "fck", "coq", "kox", "koc", "kok", "koq", 
    "diq", "dix", "dck", "pns", "psy", "fgt", "ngr", "knt", "dsh", "twt", "bch",
    "clt", "kum", "klt", "suc", "suq", "sck", "lic", "lik", "liq", "lck", "jzz", "gey",
    "gei", "gai", "vag", "vgn", "sjv", "fap", "prn", "joo", "gvr", "pis", "pss", "smn",
    "fku", "fcu", "fqu", "hor", "jap", "dyk", "dyq",
    "dyc", "jyz", "prk", "prc", "prq", "guc",
    "guc", "giz", "gzz", "sex", "sxx", "sxi", "sxe", "sxy", "xxx",
    "pot", "thc", "vaj", "vjn", "std", "lsd", "pcp", "dmn", "dam",
    "orl", "anl", "muf", "mff", "phk", "phc", "phq", "xtc",
    "mlf", "sac", "sak", "saq", "pms", "nad", "ndz", "nds", "wtf",
    "sol", "sob", "fob", "sfu", "nga", "nip"};
    bool connected = false;
    bool canSubmit = true;

    void Start()
    {
        nameInput = initialInput.GetComponent<TMP_InputField>();
        gm = FindObjectOfType<GameManager>();
        submitButton.interactable = false;
    }

    public void UpdateLeaderboard()
    {
        StartCoroutine(LoadScores());
    }

    IEnumerator LoadScores()
    {
        UnityWebRequest www = UnityWebRequest.Get("http://www.budlu.epizy.com/test.php");
        www.SetRequestHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        www.SetRequestHeader("accept-language", "en-US,en;q=0.9");
        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log("Error: " + www.error);
            connected = false;
            heading.text = "No Connection";
            initialInput.SetActive(false);
            for (int i = 0; i < 5; i++)
            {
                topFive[i].text = "" + (i + 1) + ". --- - ---";
            }
        }
        else
        {
            heading.text = "Leaderboard";
            connected = true;
            string result = www.downloadHandler.text;
            ParseText(result);
        }
    }

    private void ParseText(string text)
    {
        Debug.Log(text);
        Debug.Log(text.Substring(0, 11));
        if(text.Substring(0, 11).Equals("scores:<br>"))
        {
            text = text.Substring(11);
            Debug.Log(text);
        }
        else
        {
            Debug.Log($"Connection failed: ParseText({text})");
            connected = false;
            heading.text = "No Connection";
            initialInput.SetActive(false);
            for (int i = 0; i < 5; i++)
            {
                topFive[i].text = "" + (i + 1) + ". --- - ---";
            }
            return;
        }

        int k = 0;

        while (k < 5 && !text.Equals(""))
        {
            int brInd = text.IndexOf("<br>");
            Debug.Log(brInd);
            names[k] = text.Substring(0, 3);
            Debug.Log(names[k]);
            scores[k] = text.Substring(3, brInd - 3);
            Debug.Log(scores[k]);
            topFive[k].text = string.Format("{0}. {1} - {2}", k + 1, names[k], scores[k]);
            Debug.Log(topFive[k].text);
            text = text.Substring(brInd + 4);
            Debug.Log(text);
            k++;
        }
    }

    public void CheckInitialInput()
    {
        bool activeInput = false;
        int playerScore = FindObjectOfType<GameManager>().GetPresents();

        foreach(string score in scores)
        {
            if (playerScore > int.Parse(score))
                activeInput = true;
        }
        initialInput.SetActive(activeInput);
    }

    public void VerifyInputs()
    {
        initialHeader.text = "Enter three initials";
        if (nameInput.text.Length != 3)
        {
            submitButton.interactable = false;
            return;
        }

        bool[] validInput = new bool[3];
        string lowerInput = nameInput.text.ToLower();

        for (int i = 0; i < 3; i++)
        {
            if (lowerInput[i] != 'a' && lowerInput[i] != 'b' && lowerInput[i] != 'c' && lowerInput[i] != 'd' && lowerInput[i] != 'e' &&
                lowerInput[i] != 'f' && lowerInput[i] != 'g' && lowerInput[i] != 'h' && lowerInput[i] != 'i' && lowerInput[i] != 'j' &&
                lowerInput[i] != 'k' && lowerInput[i] != 'l' && lowerInput[i] != 'm' && lowerInput[i] != 'n' && lowerInput[i] != 'o' &&
                lowerInput[i] != 'p' && lowerInput[i] != 'q' && lowerInput[i] != 'r' && lowerInput[i] != 's' && lowerInput[i] != 't' &&
                lowerInput[i] != 'u' && lowerInput[i] != 'v' && lowerInput[i] != 'w' && lowerInput[i] != 'x' && lowerInput[i] != 'y' &&
                lowerInput[i] != 'u' && lowerInput[i] != 'v' && lowerInput[i] != 'w' && lowerInput[i] != 'x' && lowerInput[i] != 'y' &&
                lowerInput[i] != 'z')
            {
                validInput[i] = false;
            }
            else
            {
                validInput[i] = true;
            }
        }

        if (!(validInput[0] && validInput[1] && validInput[2]))
        {
            submitButton.interactable = false;
            initialHeader.text = "Only letters";
            return;
        }

        bool isValid = true;

        foreach (string word in words)
        {
            if (lowerInput.Equals(word))
            {
                isValid = false;
                initialHeader.text = "Invalid word";
                return;
            }
        }

        submitButton.interactable = isValid;
    }

    public bool GetConnected()
    {
        return connected;
    }

    public void CheckSubmit()
    {
        if (canSubmit)
        {
            StartCoroutine(Upload());
            canSubmit = false;
        }
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("username", nameInput.text.ToUpper());
        form.AddField("score", gm.GetPresents());
        UnityWebRequest www = UnityWebRequest.Post("http://www.budlu.epizy.com/input.php", form);
        www.SetRequestHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
        www.SetRequestHeader("accept-language", "en-US,en;q=0.9");
        yield return www.SendWebRequest();

        if (www.isHttpError || www.isNetworkError)
        {
            Debug.Log("Error: " + www.error);
        }
        else
        {
            Debug.Log("Successful Upload");
            StartCoroutine(LoadScores());
        }
    }
}
