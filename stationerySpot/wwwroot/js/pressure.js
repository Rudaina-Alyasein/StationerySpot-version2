document.addEventListener('DOMContentLoaded', function () {
    console.log("hooooo");
    const chatToggleButton = document.getElementById('chatToggle');
    const chatBox = document.getElementById('chatBox');
    const sendMessageButton = document.getElementById('sendMessage');
    const messageInput = document.querySelector('.message-input textarea');
    const chatContent = document.querySelector('.chat-content');
    const chatMessage = document.getElementById('chatMessage');

    // Toggle chat visibility and show message
    chatToggleButton.addEventListener('click', function () {
        if (chatBox.style.display === 'none') {
            chatBox.style.display = 'block';
            chatMessage.style.display = 'none'; // Hide the message when the chat is open

            // جلب الرسائل عند فتح الشات
            const chatBox1 = document.getElementById('chatBox1');

            const userId = chatBox1.getAttribute('data-user-id');
            const libraryId = chatBox1.getAttribute('data-library-id');

            console.log("User ID:", userId);
            console.log("Library ID:", libraryId);

            fetch(`@Url.Action("GetMessages", "Chat")?userId=${userId}&libraryId=${libraryId}`)
                .then(response => response.json())
                .then(messages => {
                    chatContent.innerHTML = ''; // Clear previous messages
                    messages.forEach(message => {
                        const messageDiv = document.createElement('div');
                        messageDiv.classList.add('message', message.senderId === userId ? 'user-message' : 'library-message');
                        messageDiv.innerHTML = `<p>${message.message}</p>`;
                        chatContent.appendChild(messageDiv);
                    });
                    chatContent.scrollTop = chatContent.scrollHeight; // Scroll to bottom
                })
                .catch(error => console.error('Error:', error));

        } else {
            chatBox.style.display = 'none';
            chatMessage.style.display = 'block'; // Show the message when the chat is closed
        }
    });

    // Handle sending a message
    chatToggleButton.addEventListener('click', function () {
        const userId = chatBox1.getAttribute('data-user-id');
        const libraryId = chatBox1.getAttribute('data-library-id');

        fetch(`/Chat/GetMessages?userId=${userId}&libraryId=${libraryId}`)
            .then(response => response.json())
            .then(messages => {
                chatContent.innerHTML = ''; // مسح الرسائل السابقة
                messages.forEach(message => {
                    const messageDiv = document.createElement('div');
                    messageDiv.classList.add('message', message.senderId === userId ? 'user-message' : 'library-message');
                    messageDiv.innerHTML = `<p>${message.message}</p>`;
                    chatContent.appendChild(messageDiv);
                });
                chatContent.scrollTop = chatContent.scrollHeight; // التمرير للأسفل
            })
            .catch(error => console.error('Error:', error));
    });

    sendMessageButton.addEventListener('click', function () {
        const messageText = messageInput.value;
        const senderId = chatBox1.getAttribute('data-user-id');
        const receiverId = chatBox1.getAttribute('data-library-id');

        if (messageText.trim() !== "") {
            fetch('/Chat/SendMessage', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({
                    message: messageText,
                    senderId: senderId,
                    receiverId: receiverId
                })
            })
                .then(response => response.json())
                .then(data => {
                    if (data.success) {
                        // إضافة الرسالة للمحتوى
                        const userMessage = document.createElement('div');
                        userMessage.classList.add('message', 'user-message');
                        userMessage.innerHTML = `<p>${messageText}</p>`;
                        chatContent.appendChild(userMessage);
                        messageInput.value = ''; // مسح حقل النص
                    } else {
                        alert('Failed to send message.');
                    }
                })
                .catch(error => console.error('Error:', error));
        }
    });
    // جلب العناصر
    const customSelects = document.querySelectorAll('.custom-select');

    // إضافة مستمعات الأحداث لكل custom-select
    customSelects.forEach((customSelect) => {
        const selectedOption = customSelect.querySelector('.selected-option');
        const optionsList = customSelect.querySelector('.options-list');
        const optionItems = customSelect.querySelectorAll('.option-item');

        // عند النقر على العنصر الأساسي، افتح أو أغلق القائمة
        selectedOption.addEventListener('click', (e) => {
            e.stopPropagation(); // لمنع النقر من إغلاق القوائم الأخرى
            customSelect.classList.toggle('open');
        });

        // عند النقر على خيار، حدّث النص وأغلق القائمة
        optionItems.forEach((option) => {
            option.addEventListener('click', () => {
                selectedOption.innerHTML = `${option.textContent} <i class="icon-arrow-down">&#9660;</i>`;
                customSelect.classList.remove('open');
            });
        });
    });

    // إغلاق القوائم عند النقر خارجها
    document.addEventListener('click', (e) => {
        customSelects.forEach((customSelect) => {
            if (!customSelect.contains(e.target)) {
                customSelect.classList.remove('open');
            }
        });
    });

    // toast -add to cart
    const addToCartButtons = document.querySelectorAll('.add-to-cart-btn');
    const toastSuccess = new bootstrap.Toast(document.getElementById('toastSuccess'));
    if (toastSuccess && addToCartButtons) {
        console.log("hi");
    }

    // إضافة الحدث على زر "Add to Cart"
    addToCartButtons.forEach(button => {
        button.addEventListener('click', (event) => {
            event.preventDefault(); // منع السلوك الافتراضي للزر (مثل إعادة تحميل الصفحة)

            // إظهار التوست
            toastSuccess.show();
        });
    });

    // pdf-file
    const fileDrop = document.querySelector('.file-drop');
    const fileInput = document.querySelector('#pdf-upload');

    fileDrop.addEventListener('click', () => fileInput.click());
    fileInput.addEventListener('change', (event) => {
        const fileName = event.target.files[0]?.name;
        if (fileName) {
            fileDrop.innerHTML = `<p><strong>${fileName}</strong> uploaded successfully!</p>`;
        }
    });

    // Event Listeners for Options
    document.querySelector('#color-option').addEventListener('change', calculateCost);
    document.querySelector('#paper-size').addEventListener('change', calculateCost);
    document.querySelector('#copies').addEventListener('input', calculateCost);
    document.querySelector('#print-sides').addEventListener('change', calculateCost);
    document.querySelector('#delivery-option').addEventListener('change', calculateCost);

    function calculateCost() {
        // Getting User Selections
        const copies = parseInt(document.querySelector('#copies').value) || 1;
        const color = document.querySelector('#color-option').value === 'color' ? 0.25 : 0.1;
        const sides = document.querySelector('#print-sides').value === 'double' ? 1.1 : 1;
        const delivery = document.querySelector('#delivery-option').checked ? 2.5 : 0;

        // Calculate Costs
        const printingCost = copies * color * sides;
        const totalCost = printingCost + delivery;

        // Update Cost Summary
        document.querySelector('#printing-cost').innerText = `$${printingCost.toFixed(2)}`;
        document.querySelector('#delivery-cost').innerText = `$${delivery.toFixed(2)}`;
        document.querySelector('#total-cost').innerText = `$${totalCost.toFixed(2)}`;
    }
})