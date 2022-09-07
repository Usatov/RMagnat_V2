import configData from "./../config.json";

export default class RequestService {
    constructor(props) {
        // super(props);
        // eslint-disable-next-line
        this._apiBase = process.env.NODE_ENV == "production" ? configData.ServerProd : configData.ServerDev;;
    }

    async getUserData (userId) {
        // Получаем информацию о пользователе
        return fetch(this._apiBase + "api/user/get/" + userId)
            .then(response => response.json())
            .then(data => {
                return data;
            });
    }

    async getBuildingTypes() {
        return fetch(this._apiBase + "api/building/")
            .then(response => response.json())
            .then(data => {
                return data;
            });
    }

    async getBuildings() {
        const sessionId = sessionStorage.getItem("session");

        return fetch(this._apiBase + "api/building/own/" + sessionId)
            .then(response => response.json())
            .then(data => {
                return data;
            });
    }

    async addBuilding(x, y, id) {
        const sessionId = sessionStorage.getItem("session");

        const requestOptions = {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                buildingTypeId: id,
                x: x,
                y: y
            })
        };

        return fetch(this._apiBase + "api/building/add/" + sessionId, requestOptions)
            .then(response => {
                return;
            });
    }

    async upBuilding(id) {
        const sessionId = sessionStorage.getItem("session");

        return fetch(this._apiBase + "api/building/up/" + sessionId + "/" + id)
            .then(() => {
                return;
            });
    }

    async downBuilding(id) {
        const sessionId = sessionStorage.getItem("session");

        return fetch(this._apiBase + "api/building/down/" + sessionId + "/" + id)
            .then(() => {
                return;
            });
    }

    async removeBuilding(id) {
        const sessionId = sessionStorage.getItem("session");

        return fetch(this._apiBase + "api/building/remove/" + sessionId + "/" + id)
            .then(() => {
                return;
            });
    }
}