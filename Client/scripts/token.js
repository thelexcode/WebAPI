function isAuthenticated() {
    const token = localStorage.getItem('token');
    if (!token) {
        return false;
    }

    // Verifica se il token è scaduto
    try {
        const tokenData = JSON.parse(atob(token.split('.')[1]));
        const expiration = tokenData.exp * 1000; 
        const isTokenValid = Date.now() < expiration;
        return isTokenValid;
    } catch (error) {
        console.error('Error parsing token data:', error);
        return false;
    }
}

function redirectToLogin() {
    if (!isAuthenticated()) {
        window.location.href = 'welcome.html'; // Reindirizza alla pagina di login se non è autenticato
    }
}

document.addEventListener('DOMContentLoaded', function() {
    // Controlla se l'utente è autenticato quando carichi la pagina index.html
    redirectToLogin();
});
