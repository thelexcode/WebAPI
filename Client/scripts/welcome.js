// Funzione per tornare alla schermata di benvenuto
function backToWelcome() {
    document.getElementById('welcome').style.display = 'block';
    document.getElementById('registerForm').style.display = 'none';
    document.getElementById('loginForm').style.display = 'none';
    document.getElementById('message').innerHTML = '';
    document.getElementById('backToWelcomeButton').style.display = 'none';
}

// Aggiungi un event listener al bottone "Back to Welcome"
document.getElementById('backToWelcomeButton').addEventListener('click', function() {
    backToWelcome();
});



