document.addEventListener("DOMContentLoaded", function () {
    const nickname = localStorage.getItem("nickname");

    // If nickname exists in localStorage, update the href attribute of presentation links
    if (nickname) {
        updatePresentationLinks(nickname);
    } else {
        // If no nickname in localStorage, ask the user for one
        promptForNickname();
    }

    // Function to prompt the user for their nickname
    function promptForNickname() {
        Swal.fire({
            title: "Enter your nickname",
            input: "text",
            inputPlaceholder: "Your nickname",
            allowOutsideClick: false,
            allowEscapeKey: false,
            confirmButtonText: "Join",
            inputValidator: (value) => {
                if (!value) {
                    return "Nickname is required!";
                }
            }
        }).then((result) => {
            if (result.isConfirmed) {
                const nickname = result.value;
                localStorage.setItem("nickname", nickname);
                updatePresentationLinks(nickname);
            }
        });
    }

    // Function to update all presentation links with the nickname
    function updatePresentationLinks(nickname) {
        document.querySelectorAll(".presentation-link").forEach((link) => {
            let href = link.getAttribute("href");
            if (!href.includes("nickname=")) {
                link.setAttribute("href", href + encodeURIComponent(nickname));
            }
        });
    }

    // Start new presentation functionality
    document.getElementById("startPresentationBtn")?.addEventListener("click", function () {
        if (!nickname) {
            alert("Please enter your nickname first.");
            return;
        }

        // Navigate to create a new presentation with the nickname
        window.location.href = `/Presentation/Create?nickname=${encodeURIComponent(nickname)}`;
    });
});
