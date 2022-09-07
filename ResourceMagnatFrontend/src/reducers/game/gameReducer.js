const initialState = {
    user: {},
    buildingTypes: [],
    buildings: [],
    coins: 0,
    loading: true
};

const gameReducer = (state = initialState, action) => {
    switch (action.type) {
        case "LOAD_COMPLETE":
            return {
                ...state,
                buildings: action.payload,
                loading: false
            }

        case "BUILDING_TYPES":
            return {
                ...state,
                buildingTypes: action.payload
            }

        case "USER_DATA":
            return {
                ...state,
                user: action.payload,
                coins: action.payload.coins
            }

        case "TICK":
            return {
                ...state,
                coins: state.coins + state.user.coinsPerSecond,
                buildings: state.buildings
            }

        default:
            return state;
    }
}

export default gameReducer;