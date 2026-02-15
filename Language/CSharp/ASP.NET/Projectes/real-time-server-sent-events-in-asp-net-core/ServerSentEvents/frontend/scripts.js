// 1. Connect to the SSE endpoint
const source = new EventSource('http://localhost:5000/stocks');

// 2. Listen for our named "stockUpdate" events
source.addEventListener('stockUpdate', e => {
    // Parse the JSON payload
    const { symbol, price, timestamp } = JSON.parse(e.data);

    // Create and prepend a new list item with Tailwind classes
    const li = document.createElement('li');
    li.classList.add('new', 'flex', 'justify-between', 'items-center');

    // Create time element
    const timeSpan = document.createElement('span');
    timeSpan.classList.add('text-gray-500', 'text-sm');
    timeSpan.textContent = new Date(timestamp).toLocaleTimeString();

    // Create symbol element
    const symbolSpan = document.createElement('span');
    symbolSpan.classList.add('font-medium', 'text-gray-800');
    symbolSpan.textContent = symbol;

    // Create price element
    const priceSpan = document.createElement('span');
    priceSpan.classList.add('font-bold', 'text-green-600');
    priceSpan.textContent = `$${price}`;

    // Append all elements to the list item
    li.appendChild(timeSpan);
    li.appendChild(symbolSpan);
    li.appendChild(priceSpan);

    const list = document.getElementById('updates');
    list.prepend(li);

    // Remove highlight after a moment
    setTimeout(() => li.classList.remove('new'), 2000);
});

// 3. Handle errors & automatic reconnection
source.onerror = err => {
    console.error('SSE connection error:', err);
    console.log('If you see CORS errors, make sure the backend has CORS enabled');
    // Show error message to the user
    const errorMessage = document.createElement('div');
    errorMessage.classList.add('bg-red-100', 'text-red-700', 'p-4', 'rounded', 'mb-4');
    errorMessage.textContent = 'Connection error. Check console for details.';

    // Only add the error message if it doesn't exist yet
    if (!document.querySelector('.bg-red-100')) {
        document.querySelector('.max-w-4xl').prepend(errorMessage);
    }

    // The browser will retry automatically using the last seen ID
};

// 4. (Optional) Inspect the last-received event ID
source.onmessage = e => {
    console.log('Last Event ID now:', source.lastEventId);
};
