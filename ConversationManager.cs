using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConversationManager : Singleton<ConversationManager>
{
    protected ConversationManager() { } // guarantee this will be always a singleton only - can't use the constructor!

    //Is there a conversation going on
    static bool talking = false;

    //The current line of text being displayed
    ConversationEntry currentConversationLine;

    //the Canvas Group for the dialog box
    public CanvasGroup dialogBox;

    //the text holder
    public Text textHolder;

    // velocidad de despliegue de los caracteres
    private float velDespliegueChar = 0.03f;



    public void StartConversation(Conversation conversation,  LLT.Variables.FloatVariable numPensamiento)
    {
       // Debug.Log("StartConversation");
        dialogBox = UnityEngine.GameObject.Find("DialogBox").GetComponent<CanvasGroup>();
        textHolder = UnityEngine.GameObject.Find("DialogText").GetComponent<Text>();
            DisplayConversation(conversation, numPensamiento);
   
        
        if (dialogBox != null && textHolder != null)
        {
           // Debug.Log("estan los elementos para la conversacion ");
            //Start displying the supplied conversation
            if (!talking)
            {
             //   Debug.Log("empiezo conversacion ");
                talking = true;
                DisplayConversation(conversation, numPensamiento);
                //StartCoroutine(DisplayConversation(conversation));
            }
        }
    }
   
    public void EndConversation()
    {
 
        talking = false;
        StopAllCoroutines();
        
    }

    
     public void DisplayConversation(Conversation conversation, LLT.Variables.FloatVariable numPensamiento)
      {
        Scene m_Scene;

        m_Scene = SceneManager.GetActiveScene();
        
        if (m_Scene.name == "CeldaBlur" || m_Scene.name == "Celda")
        {
            Debug.Log("En " + m_Scene.name);
           // if (numPensamiento.Value == 0f ) return;
            
            currentConversationLine = conversation.ConversationLines[(int)numPensamiento.Value];
            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentConversationLine.ConversationText));
           // Debug.Log("Esta es la linea de la celda " + currentConversationLine.ConversationText);
        }
        else
        {
            Debug.Log("Estoy en " + m_Scene.name);
            foreach (var conversationLine in conversation.ConversationLines)
            {
                currentConversationLine = conversationLine;
                if (currentConversationLine != null)
                {

                    StopAllCoroutines();
                    StartCoroutine(TypeSentence(currentConversationLine.ConversationText));
                 //   Debug.Log("Esta es la linea " + currentConversationLine.ConversationText);
                }



            }
        }
      }

        IEnumerator TypeSentence(string sentence)
    {
       // if (talking == false) yield return null;
        if (textHolder == null)
        {
            yield return null;
        }
        textHolder.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            if (talking == false) yield return null;
            if (textHolder == false) yield return null;
            textHolder.text += letter;

            //yield return null;
            yield return new WaitForSeconds(velDespliegueChar);
        }
        // esto es nuevo ojo
        
        // esto lo quito para que el pensamiento desaparezca cuando se entra a otro objeto
        /*
        yield return new WaitForSeconds(1f);
        */

       // talking = false;
        
        //
    }


    void OnGUI()
    {
        // voy a modificar porque me sale error de que dialogBox ya no existe.
        //if (talking)
        if(talking && dialogBox != null)
        {
            // esto es nuevo, a ver si modifico los tamaños de los contenedores
            RectTransform rT = UnityEngine.GameObject.Find("DialogBox").GetComponent<RectTransform>();
            float numLineas = (float)currentConversationLine.ConversationText.Length / 50;
            if (numLineas == 0) numLineas = 1f;

            rT.sizeDelta = new Vector2(rT.sizeDelta.x, ((numLineas +1) * 50) + 20);

            rT = UnityEngine.GameObject.Find("Viewport").GetComponent<RectTransform>();
            numLineas = (float)currentConversationLine.ConversationText.Length / 50;
            if (numLineas == 0) numLineas = 1f;

            rT.sizeDelta = new Vector2(rT.sizeDelta.x, (numLineas +1) * 50);

            dialogBox.alpha = 1;
            dialogBox.blocksRaycasts = true;

            
        }
        else  // talking = false;
        {
            // aqui no estaba este if, solo las lineas de dialogBox...
            if (dialogBox != null)
            {
                dialogBox.alpha = 0;
                dialogBox.blocksRaycasts = false;
            }
            
        }
    }

    public void ParaCorrutinaConversation()
    {
        StopAllCoroutines();
    }

    public bool ObtenTalking()
    {
        return talking;
    }
}
