using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    // Title screen functionality.

    //

    [SerializeField] GameObject title;
    [SerializeField] GameObject menuOptionsDisplay;
    [SerializeField] GameObject credits;

    GameObject cursor;
    [SerializeField] float cursorXPosition = -3f;
    [SerializeField] float cursorStartingYPosition = -1.1f;
    [SerializeField] float cursorStep = 0.75f;

    enum MenuScreens {Main, Options, Credits}
    MenuScreens currentSubMenu = MenuScreens.Main;

    enum MenuOptions {Start, Options, Credits, Quit}
    MenuOptions currentOptionHovered = MenuOptions.Start;

    [SerializeField] InputAction menu;
    float menuValue;

    [SerializeField] InputAction confirm;
    float confirmValue;

    bool activeInput = false;
    bool inputAlreadyMade = false;

    void OnEnable()
    {
        menu.Enable();
        confirm.Enable();
    }

    void OnDisable()
    {
        menu.Disable();
        confirm.Disable();
    }

    void Start()
    {
        cursor = GameObject.FindWithTag("UI Cursor");
        if (AnImportantObjectIsNull()) return;
        ResetUIObjectActivity();
        PositionCursor();
    }
    
    void Update()
    {
        if (AnImportantObjectIsNull()) return;
        HandleInput();

        switch (currentSubMenu)
        {
            case MenuScreens.Main: // In main menu.
                if (activeInput && !inputAlreadyMade)
                {
                    if (menuValue != 0f)
                    {
                        currentOptionHovered = (MenuOptions)Mathf.Repeat((float)currentOptionHovered + Mathf.Round(menuValue * -1f), 4f);
                        PositionCursor();
                    }
                    if (confirmValue != 0f)
                    {
                        switch (currentOptionHovered)
                        {
                            case MenuOptions.Start: // Confirm input on "Start Game" option.
                                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                                break;
                            case MenuOptions.Credits: // Confirm input on "Credits" option.
                                title.SetActive(false);
                                menuOptionsDisplay.SetActive(false);
                                credits.SetActive(true);
                                cursor.SetActive(false);

                                currentSubMenu = MenuScreens.Credits;
                                break;
                            case MenuOptions.Quit: // Confirm input on "Quit" option.
                                Application.Quit();
                                break;
                            default:
                                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                                break;
                        }
                    }
                }
                break;
            case MenuScreens.Credits: // On Credits screen.
                if (activeInput && !inputAlreadyMade && (confirmValue != 0f))
                {
                    ResetUIObjectActivity();
                    currentSubMenu = MenuScreens.Main;
                }
                break;
            default:
                if (activeInput && !inputAlreadyMade)
                {
                    ResetUIObjectActivity();
                    currentSubMenu = MenuScreens.Main;
                }
                break;
        }
    }

    bool AnImportantObjectIsNull()
    {
        return ((title == null) || (menuOptionsDisplay == null) || (credits == null) || (cursor == null));
    }

    void HandleInput()
    {
        inputAlreadyMade = activeInput;
        menuValue = menu.ReadValue<float>();
        confirmValue = confirm.ReadValue<float>();
        activeInput = ((menuValue != 0f) || (confirmValue != 0f));
    }

    void ResetUIObjectActivity()
    {
        title.SetActive(true);
        menuOptionsDisplay.SetActive(true);
        credits.SetActive(false);
        cursor.SetActive(true);
    }

    void PositionCursor()
    {
        cursor.transform.position = new Vector3(cursorXPosition, cursorStartingYPosition - (cursorStep * (float)(currentOptionHovered)), 0f);
    }
}
