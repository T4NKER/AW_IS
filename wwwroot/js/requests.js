
document.addEventListener("DOMContentLoaded", function () {
    assignButtons();
    checkDataSize();
    checkDeadLines();
});


var modal = document.getElementById("requestModal");

var btn = document.getElementById("submitRequestBtn");

var form = document.querySelector("#requestModal form");

var span = document.getElementsByClassName("close")[0];

btn.onclick = function () {
    modal.style.display = "block";
}

span.onclick = function () {
    modal.style.display = "none";
}

window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

form.addEventListener('submit', async function (event) {
    event.preventDefault();
    var deadlineTime = new Date(document.getElementById('deadlineTime').value);

    var currentTime = new Date();
    var oneHourFromNow = new Date(currentTime.getTime() + (3600 * 1000));

    if (deadlineTime <= currentTime || deadlineTime <= oneHourFromNow) {
        alert("Deadline time cannot be in the past or less than one hour from now.");
        return;
    }

    var formData = new FormData(form);
    try {
        const response = await fetch('/Request/CreateRequest', {
            method: 'POST',
            body: formData
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        modal.style.display = "none";
        form.reset();
    } catch (error) {
        console.error('Error submitting request:', error);
    }
});




document.querySelectorAll('.table tbody tr').forEach(row => {
    row.addEventListener('click', function (event) {
        var doneButton = row.querySelector('.request-done-btn');
        if (event.target == doneButton) {
            return;
        }
        expandRow(this);
    });
});


function assignButtons() {
    document.querySelectorAll(".request-done-btn").forEach(button => {
        button.addEventListener("click", function (event) {
            event.preventDefault();
            var id = this.getAttribute('data-id');
            markAsDone(id);
        });
    });
}



document.getElementsByClassName("prev-page-btn")[0].addEventListener("click", function (event) {
    event.preventDefault();
    paginationButtons("prev");
})

document.getElementsByClassName("next-page-btn")[0].addEventListener("click", function (event) {
    event.preventDefault();
    paginationButtons("next");
})



// The purpose of this is to update the table when a request is marked as done for any user that is currently viewing the Requests.
// I tried to make it as non invasive as possible, but the goal of the SSE here is to instantly get to the support any requests
// that have been added or deleted. That being said, the user will stay on the same page unless there isn't any data to display 
// on that page in which case it goes to the previous page i.e Request/RequestView/2 ==> no data displayed ==> Request/RequestView/1

// NOTE: Beforehand it was only updated for one user meaning that when an user made a DeleteRequest or CreateRequest api call, 
// they would see the change reflected on the page only for themselves. Now it is updated for all users.


var eventSource = new EventSource('/request-updates');

eventSource.onopen = function(event) {
    console.log("SSE open")
};

eventSource.onmessage = function(event) {
    console.log("Logging SSE message", event)
};

eventSource.onerror = function(event) {
    console.error('SSE error:', event);
    eventSource.close();
};  

eventSource.addEventListener('requests_updated', function (event) {
    location.reload()
    
});

function checkDeadLines() {
    let date = new Date();
    let sec = date.getSeconds();
    setTimeout(() => {
        setInterval(() => {
            var now = new Date();

            document.querySelectorAll('tr').forEach(row => {

                var deadlineTimeString = row.cells[3].innerText;

                var parts = deadlineTimeString.split('/');

                var temp = parts[0];
                parts[0] = parts[1];
                parts[1] = temp;

                deadlineTimeString = parts.join('/');

                var deadlineTime = new Date(deadlineTimeString);

                var oneHourFromNow = new Date(now.getTime() + (3600 * 1000));
                console.log("Deadline check" + deadlineTime + " " + oneHourFromNow);
                if (deadlineTime <= oneHourFromNow) {
                    console.log("Deadline has passed" + deadlineTime + " " + oneHourFromNow);
                    row.style.background = "red";
                }
            });
        }, 60 * 1000);
    }, (60 - sec) * 1000);
};

function paginationButtons(type) {
    var currentPage = parseInt(window.location.pathname.split('/').pop());

    if (!isNaN(currentPage)) {
        if (type === "next") {
            currentPage = currentPage + 1;
        } else if (type === "prev" && currentPage > 1) {
            currentPage = currentPage - 1;
        }
        var nextPageURL = currentPage > 1 ? `/Request/RequestView/${currentPage}` : `/Request/RequestView/1`;
        window.location.href = nextPageURL;

    } else {
        console.error('Invalid current page:', currentPage);
    }
}

async function markAsDone(id) {
    event.preventDefault();
    idInt = parseInt(id);

    try {
        const response = await fetch(`/Request/DeleteRequest/?id=${idInt}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        console.log('Request successfully marked as done:', data);
    } catch (error) {
        console.error('Error marking request as done:', error);
    }
}

function expandRow(HTMLelement) {
    var description = HTMLelement.querySelector('.description').innerText;

    var existingExpandedRow = HTMLelement.nextElementSibling;
    if (existingExpandedRow && existingExpandedRow.classList.contains('expanded-row')) {
        existingExpandedRow.remove();
        HTMLelement.classList.remove('expanded');
    } else {
        var expandedRow = document.createElement('tr');
        expandedRow.classList.add('expanded-row');


        var descriptionCell = document.createElement('td');
        descriptionCell.setAttribute('colspan', '5');
        descriptionCell.classList.add('expanded-description');
        descriptionCell.textContent = description;

        expandedRow.appendChild(descriptionCell);

        HTMLelement.insertAdjacentElement('afterend', expandedRow);

        HTMLelement.classList.add('expanded');
    }
}

function checkDataSize() {
    var rowCount = document.querySelectorAll('.table tbody tr').length;
    var currentPage = parseInt(window.location.pathname.split('/').pop());

    if (rowCount < 10) {
        var nextButton = document.querySelector('.next-page-btn');
        if (nextButton) {
            nextButton.disabled = true;
        }
    }
    if (currentPage == 1) {
        var prevButton = document.querySelector('.prev-page-btn');
        if (prevButton) {
            prevButton.disabled = true;
        }
    }

}