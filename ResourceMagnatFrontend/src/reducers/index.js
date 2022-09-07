import { combineReducers } from "redux";
// import { connectRouter } from "connected-react-router";

import gameReducer from "./game/gameReducer";

const reducers = {
    game: gameReducer
}

const createRootReducer = () =>
    combineReducers({
        // router: connectRouter(history),
        ...reducers
    });

export default createRootReducer;
