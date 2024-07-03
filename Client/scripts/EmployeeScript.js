$(document).ready(function() {
    loadEmployees();

    // Aggiungi un nuovo employee
    $('#addEmployeeForm').submit(function(event) {
        event.preventDefault();
        
        var formData = {
            FirstName: $('#firstName').val(),
            LastName: $('#lastName').val(),
            Email: $('#email').val(),
            Department: $('#department').val()
        };

        fetch('https://localhost:7154/api/employee', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(formData)
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            $('#addEmployeeModal').modal('hide');
            loadEmployees();
        })
        .catch(error => {
            console.error('Error:', error);
            // Gestisci l'errore qui, ad esempio mostrando un messaggio all'utente
        });
    });

    // Carica tutti gli employees
    function loadEmployees() {
        fetch('https://localhost:7154/api/employee')
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            displayEmployees(data);
        })
        .catch(error => {
            console.error('Error:', error);
            // Gestisci l'errore qui, ad esempio mostrando un messaggio all'utente
        });
    }

    // Mostra gli employees nella tabella
    function displayEmployees(employees) {
        var tableBody = $('#employeesTable tbody');
        tableBody.empty();

        employees.forEach(employee => {
            var row = `
                <tr>
                    <td>${employee.firstName}</td>
                    <td>${employee.lastName}</td>
                    <td>${employee.email}</td>
                    <td>${employee.department}</td>
                    <td>
                        <button type="button" class="btn btn-primary btn-edit" data-toggle="modal" data-target="#editEmployeeModal" data-employee-id="${employee.id}">Edit</button>
                        <button type="button" class="btn btn-danger btn-delete" data-employee-id="${employee.id}">Delete</button>
                    </td>
                </tr>
            `;
            tableBody.append(row);
        });
    }

    // Event listener per il pulsante Elimina su ogni riga della tabella
    $('#employeesTable').on('click', '.btn-delete', function() {
        var employeeId = $(this).data('employee-id');

        fetch(`https://localhost:7154/api/employee/${employeeId}`, {
            method: 'DELETE'
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            loadEmployees();
        })
        .catch(error => {
            console.error('Error:', error);
            // Gestisci l'errore qui, ad esempio mostrando un messaggio all'utente
        });
    });

    // Event listener per il pulsante Modifica su ogni riga della tabella
    $('#employeesTable').on('click', '.btn-edit', function() {
        var employeeId = $(this).data('employee-id');

        // Carica i dettagli dell'employee nel form di modifica
        fetch(`https://localhost:7154/api/employee/${employeeId}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(employee => {
            // Popola il form di modifica con i dati dell'employee
            $('#editEmployeeId').val(employee.id);
            $('#editFirstName').val(employee.firstName);
            $('#editLastName').val(employee.lastName);
            $('#editEmail').val(employee.email);
            $('#editDepartment').val(employee.department);

            // Mostra il modal di modifica
            $('#editEmployeeModal').modal('show');
        })
        .catch(error => {
            console.error('Error:', error);
            // Gestisci l'errore qui, ad esempio mostrando un messaggio all'utente
        });
    });

    // Modifica un employee
    $('#editEmployeeForm').submit(function(event) {
        event.preventDefault();

        var formData = {
            Id: $('#editEmployeeId').val(),
            FirstName: $('#editFirstName').val(),
            LastName: $('#editLastName').val(),
            Email: $('#editEmail').val(),
            Department: $('#editDepartment').val()
        };

        fetch(`https://localhost:7154/api/employee/${formData.Id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(formData)
        })
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            $('#editEmployeeModal').modal('hide');
            loadEmployees();
        })
        .catch(error => {
            console.error('Error:', error);
            // Gestisci l'errore qui, ad esempio mostrando un messaggio all'utente
        });
    });
});
