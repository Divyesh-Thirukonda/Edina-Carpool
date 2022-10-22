using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using TMPro;

public class FBscript : MonoBehaviour
{
    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;    
    public FirebaseUser User;
    public DatabaseReference DBreference;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField phoneRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;

    public TMP_InputField startField;
    public TMP_InputField endField;
    


    public GameObject[] allThingsToDisable;
    public GameObject DatabaseStuff;
    
    public GameObject scoreElement;
    public Transform scoreboardContent;

    void Awake()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
        
    }

    //Function for the scoreboard button
    public void ScoreboardButton()
    {        
        StartCoroutine(LoadScoreboardData());
    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            Debug.Log(message);
        }
        else
        {
            User = LoginTask.Result;

            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            foreach (GameObject games in allThingsToDisable) {
                games.SetActive(false);
            }
            DatabaseStuff.SetActive(true);
            InvokeRepeating("ScoreboardButton", .1f, 10f);
            StartCoroutine(UpdateName(User.DisplayName));
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            Debug.Log("Missing Username");
        }
        else 
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                Debug.Log(message);
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;
                StartCoroutine(UpdatePhone(phoneRegisterField.text));
                StartCoroutine(UpdateName(User.DisplayName));
                StartCoroutine(UpdateStart(""));
                StartCoroutine(UpdateEnd(""));

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile{DisplayName = _username};

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        Debug.Log("Username Set Failed!");
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        UIManager.instance.LoginScreen();
                        
                    }
                }
            }
        }
    }



    private IEnumerator UpdatePhone(string _phone)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("Phone").SetValueAsync(_phone);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            Debug.Log("Updated");
        }
    }

    private IEnumerator UpdateStart(string _start)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("Start").SetValueAsync(_start);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            Debug.Log("Updated");
        }
    }

    private IEnumerator UpdateEnd(string _end)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("End").SetValueAsync(_end);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            Debug.Log("Updated");
        }
    }

    private IEnumerator UpdateName(string username)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            Debug.Log("Updated");
        }
    }

    private IEnumerator LoadScoreboardData()
    {
        //Get all the users data ordered by kills amount
        var DBTask = DBreference.Child("users").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in scoreboardContent.transform)
            {
                Destroy(child.gameObject);
            }

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                string username = User.DisplayName;
                string start = childSnapshot.Child("Start").Value.ToString();
                string end = childSnapshot.Child("End").Value.ToString();
                string phone = childSnapshot.Child("Phone").Value.ToString();

                //Instantiate new scoreboard elements
                GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                Debug.Log("Instantiated");
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, start, phone, end);

                // instanceToDestroy = scoreboardElement;
            }

            //Go to scoareboard screen
            UIManager.instance.ScoreboardScreen();
            Debug.Log("Doned");
        }
    }

    GameObject instanceToDestroy;


    // public void DeleteInstance(GameObject instance) {
    //     Destroy(instance);
    // }


    public void SaveButton() {
        StartCoroutine(UpdateStart(startField.text));
        StartCoroutine(UpdateEnd(endField.text));
        StartCoroutine(UpdateName(User.DisplayName));
    }



    public void GetPhoneNumberEnum() {
         StartCoroutine(GetPhone());
    }

    private IEnumerator GetPhone()
    {
        var DBTask = DBreference.Child("users").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            foreach (DataSnapshot childSnapshot in snapshot.Children)
            {
                mailBodyBig = "Hey, " + User.DisplayName + " has offered to pick you up! His phone number is " + childSnapshot.Child("Phone").Value.ToString() + ". Let them know whether you want to accept the ride or decline. Thank you for using Edina Uber!";
                DoBig(mailBodyBig);

                // yield return new WaitForSeconds(30);
                // DeleteInstance(instanceToDestroy);
            }
        }
    }

    public static string mailBodyBig;
    public static string msg;

    void DoBig(string mailBodyBigger) {
        msg = mailBodyBigger;
    }







}
