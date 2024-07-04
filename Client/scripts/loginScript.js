document.addEventListener('DOMContentLoaded', function() {
    // Mostra il form di login e nasconde gli altri elementi
    function showLoginForm() {
        document.getElementById('welcome').style.display = 'none';
        document.getElementById('registerForm').style.display = 'none';
        document.getElementById('loginForm').style.display = 'block';
        document.getElementById('backToWelcomeButton').style.display = 'block';
    }

    // Aggiungi un event listener al bottone di login, se esiste
    const loginButton = document.getElementById('loginButton');
    if (loginButton) {
        loginButton.addEventListener('click', function() {
            showLoginForm();
        });
    }

    // Aggiungi un event listener per il submit del form di login, se esiste
    const loginForm = document.getElementById('loginFormContent');
    if (loginForm) {
        loginForm.addEventListener('submit', async function(event) {
            event.preventDefault();

            const email = document.getElementById('loginEmail').value;
            const password = document.getElementById('loginPassword').value;

            try {
                // Invia la richiesta al backend con passwordHash
                const response = await fetch('https://localhost:7154/api/login', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        email: email,
                        passwordHash: password 
                    })
                });

                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }

                // Estrai il token JWT dalla risposta
                const data = await response.json();
                const token = data.token;

                // Salva il token nel localStorage
                localStorage.setItem('token', token);

                // Login successful, redirect to index.html
                window.location.href = 'index.html';
            } catch (error) {
                console.error('There was an error logging in:', error);
                const messageElement = document.getElementById('message');
                messageElement.innerHTML = `<div class="alert alert-danger">Login failed: ${error.message}</div>`;
            }
        });
    }
});
