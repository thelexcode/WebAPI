// Mostra il form di registrazione e nasconde gli altri elementi
function showRegisterForm() {
    document.getElementById('welcome').style.display = 'none';
    document.getElementById('loginForm').style.display = 'none';
    document.getElementById('registerForm').style.display = 'block';
    document.getElementById('backToWelcomeButton').style.display = 'block';
}

// Aggiungi un event listener al bottone di registrazione
document.getElementById('registerButton').addEventListener('click', function() {
    showRegisterForm();
});

// Aggiungi un event listener per il submit del form di registrazione
document.getElementById('registerFormContent').addEventListener('submit', async function(event) {
    event.preventDefault();

    const firstName = document.getElementById('firstName').value;
    const lastName = document.getElementById('lastName').value;
    const email = document.getElementById('email').value;
    const password = document.getElementById('password').value;

    try {
        const response = await fetch('https://localhost:7154/api/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                FirstName: firstName,
                LastName: lastName,
                Email: email,
                Password: password
            })
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }

        const messageElement = document.getElementById('message');
        messageElement.innerHTML = `<div class="alert alert-success">Registration successful. You can now <a href="#" onclick="showLoginForm()">login</a>.</div>`;

    } catch (error) {
        console.error('There was an error registering:', error);
        const messageElement = document.getElementById('message');
        messageElement.innerHTML = `<div class="alert alert-danger">Registration failed: ${error.message}</div>`;
    }
});
