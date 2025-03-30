document.addEventListener("DOMContentLoaded", function () {
    let currentSlideId = parseInt(document.querySelector("#slides-panel li")?.dataset.slideId) || 0;

    // ✅ SignalR Connection
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/presentationHub")
        .build();

    connection.start().then(() => {
        console.log("Connected to presentation hub");
        connection.invoke("JoinPresentation", presentationId, nickname);
    }).catch(err => console.error("SignalR Connection Error:", err));

    // ✅ Listen for Text Block Updates
    connection.on("TextBlockUpdated", (slideId, textBlockId, newMarkdownText) => {
        if (slideId !== currentSlideId) return;
        const block = document.querySelector(`[data-textblock-id='${textBlockId}']`);
        if (block) block.innerHTML = marked.parse(newMarkdownText);
    });

    // ✅ Handle New Users Joining
    connection.on("UserJoined", (nicknameJoined) => {
        console.log(`${nicknameJoined} joined the presentation.`);
        const userList = document.getElementById("userList");
        if (!userList) return;

        if (![...userList.children].some(li => li.textContent === nicknameJoined)) {
            const li = document.createElement("li");
            li.textContent = nicknameJoined;
            userList.appendChild(li);
        }
    });

    // ✅ Switch Slide
    document.querySelectorAll("#slides-panel li").forEach(slide => {
        slide.addEventListener("click", function () {
            currentSlideId = parseInt(this.dataset.slideId);
            loadSlide(currentSlideId);
        });
    });

    function loadSlide(slideId) {
        fetch(`/api/slides/${slideId}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Slide not found');
                }
                return response.json();
            })
            .then(data => {
                document.querySelector("#slide-area").innerHTML = data.html;
            })
            .catch(err => {
                console.error("Error loading slide:", err);
                document.querySelector("#slide-area").innerHTML = "<p>Failed to load slide content.</p>";
            });
    }

    // ✅ Add New Slide (Only for Creator)
    document.getElementById("addSlideBtn")?.addEventListener("click", function () {
        fetch(`/api/slides/create`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ presentationId: presentationId })
        })
            .then(response => response.json())
            .then(newSlide => {
                const slideList = document.querySelector("#slides-panel ul");
                const li = document.createElement("li");
                li.dataset.slideId = newSlide.id;
                li.textContent = newSlide.title;
                slideList.appendChild(li);
            })
            .catch(err => console.error("Error adding slide:", err));
    });

    // ✅ Save Text Block Updates
    window.updateTextBlock = function (textBlockId, newText) {
        fetch(`/api/slides/${currentSlideId}/textblocks/${textBlockId}`, {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ markdownText: newText })
        });

        connection.invoke("UpdateTextBlock", currentSlideId, textBlockId, newText)
            .catch(err => console.error("Error sending text block update:", err));
    };

    // ✅ Start New Presentation
    document.getElementById("startPresentationBtn")?.addEventListener("click", function () {
        fetch(`/api/presentations/create`, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ title: "New Presentation", creatorNickname: nickname })
        })
            .then(response => response.json())
            .then(presentation => {
                window.location.href = `/Presentation/Create?presentationId=${presentation.id}&nickname=${nickname}`;
            })
            .catch(err => console.error("Error creating presentation:", err));
    });
});

document.getElementById("updateTitleBtn")?.addEventListener("click", function () {
    const newTitle = prompt("Enter new presentation title:", document.title);
    if (newTitle && newTitle !== document.title) {
        fetch('/api/presentation/UpdateTitle', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                presentationId: presentationId, // Pass your presentation ID here
                newTitle: newTitle
            })
        })
            .then(response => response.json())
            .then(data => {
                document.title = data.title; // Update the title on the page
                Swal.fire('Success', 'Presentation title updated!', 'success');
            })
            .catch(err => {
                console.error('Error updating title:', err);
                Swal.fire('Error', 'Failed to update title', 'error');
            });
    }
});

document.getElementById("saveSlideBtn")?.addEventListener("click", function () {
    const newContent = document.getElementById("slide-area").innerHTML;

    fetch(`/api/slides/${currentSlideId}/content`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ content: newContent })
    })
        .then(response => {
            if (response.ok) {
                Swal.fire('Success', 'Slide content saved!', 'success');
            } else {
                Swal.fire('Error', 'Failed to save slide content', 'error');
            }
        })
        .catch(err => {
            console.error("Error saving slide:", err);
            Swal.fire('Error', 'Failed to save slide content', 'error');
        });
});