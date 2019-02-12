# Sherpany UWP Code Challenge
Preface
This App is absolutely useless! At least that’s what we want you to think. Sherpany handles very sensitive information, thus security is key. This App will enable you to securely download and store our values.

Communication:
Please keep us informed about your progress, for example when you start or end your challenge. Just to help us understand how you work. If you find another way of letting us know your work-style, please do so.


1. Refactoring

The empty MainPageViewModel is already set as DataContext of the MainPage. Move and refactor all code according to the MVVM pattern. Please make sure all following tasks stick to the pattern too.

2. Drag to reveal the login

I’m sure you already noticed, the button currently hides the TextBox behind it. Enable the user to freely drag DragableGrid away (any direction) to reveal the secret input underneath it.

3. Set the pin and navigate

Make sure the user enters a six digit Pin. If the input is valid enable the button. Clicking the button will store the pin (securely) and Navigate to the SherpanyValuesPageView. Implement the IKeyManager interface to achieve this.

4. If the Pin has been set once. Try to retrieve it on start-up.

The MainPage input should say “Please confirm your pin:” and instead of setting a new Pin check if the user enters the correct pin. The “Navigate” button will only work with a valid Pin.

5. Show the values in SherpanyValuesPageView

Use the ApiService to retrieve the List, consider the load time is up to 5 seconds.
The Page should show a list of our values (only title) considering the order. Only one item can be selected, selecting it will expand it and show the description and claim.

6. Store the list securely

Sherpany also works without calls to the Api. After having downloaded the objects from the api once, the app will store them offline. When you start the app the next time, if the Pin of a user is already set, instead of calling the Api again retrieve the SherpanyValueModels from the LocalState. Make sure to encrypt everything with the Pin. You can use the EncryptionProvider, but you don’t have to.

7. Drag and Drop to change order

The order can be changed by drag and drop, changes are stored and the order can be retrieved after reopening the app.


8. Unit tests

Create unit tests for the ValidatePasswordAndNavigateCommand. Create a new Unit Test project and use your favorite unit test framework.



Impress us (not mandatory):

We are interested in improving this code challenge. Make up your own exercise. What skill do you think is important for an UWP developer? Can you write a creative task, that can be done within an hour?
