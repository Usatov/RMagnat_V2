// import { createBrowserHistory } from 'history';
import { createStore } from 'redux';
import createRootReducer from "../reducers/index";

// export const history = createBrowserHistory();

export const store = createStore(createRootReducer());