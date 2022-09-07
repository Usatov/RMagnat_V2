import 'bootstrap/dist/css/bootstrap.css';
import React from 'react';
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import { store } from './store/store';
import App from './components/pages/app/App';

const rootElement = document.getElementById('root');
const root = createRoot(rootElement);

root.render(
    <Provider store={store}>
        <App />
    </Provider>
);
