import React, { Component } from 'react';
import { v4 as uuid } from 'uuid';
import { store } from './../../store/store';
import { connect } from 'react-redux';
import { Game } from '../game/Game'
import RequestService from "../../services/requestService";
import './Home.css';

class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        // this.state = { user: {}, buildingTypes: [], buildings: [], coins: 0, loading: true };
        this.state = { loading: true };
        this.requestService = new RequestService();
    }

    componentDidMount() {
        setInterval(() => {
            store.dispatch({ type: 'TICK'});
        }, 1000);
        this.getUserData();
    }

    getUserId() {
        let user_uid = localStorage.getItem('user_uid');
        if (user_uid == null) {
            user_uid = uuid();
            localStorage.setItem('user_uid', user_uid);
        }
        return user_uid;
    }

    separateComma(val) {
        let sign = 1;
        if (val < 0) {
            sign = -1;
            val = -val;
        }
        
        let num = val.toString().includes('.') ? val.toString().split('.')[0] : val.toString();
        let len = num.toString().length;
        let result = '';
        let count = 1;

        for (let i = len - 1; i >= 0; i--) {
            result = num.toString()[i] + result;
            if (count % 3 === 0 && count !== 0 && i !== 0) {
                result = ',' + result;
            }
            count++;
        }

        
        if (val.toString().includes('.')) {
            result = result + '.' + val.toString().split('.')[1];
        }
        
        return sign < 0 ? '-' + result : result;
    }


    render() {
        if (store == null) return (<div/>);
        const state = store.getState().game;

        if (state.loading) {
            return (
                <div className="center">
                    Загрузка...
                </div>
            );
        } else {
            return (
                <div className="center">
                    <h1>Добытчик ресурсов</h1>
                    <span style={{ ver: "inline-block" }}>
                        <img src={process.env.PUBLIC_URL + '/img/Coin.png'}
                            style={{ width: 32, height: 32, background: "transparent", display: "inline-block" }}
                            alt="Монеты" />
                        <h2 style={{ display: "inline-block", verticalAlign: "middle", marginLeft: 10 }}>{this
                            .separateComma(state.coins)}</h2>
                        <h4>{state.user.coinsPerSecond} / сек.</h4>
                    </span>
                    <Game buildings={state.buildings} owner={this} />
                </div>
            );
        }
    }

    async getUserData() {
        const data = await this.requestService.getUserData(this.getUserId());
        sessionStorage.setItem("session", data.sessionId);
        store.dispatch({ type: 'USER_DATA', payload: data });

        // this.setState({ user: data, coins: data.coins });
        this.getBuildings();
    }

    async getBuildings() {
        const state = store.getState().game;

        if (state.buildingTypes == null || state.buildingTypes.length == 0) {
            const dataBuildingTypes = await this.requestService.getBuildingTypes();
            store.dispatch({ type: 'BUILDING_TYPES', payload: dataBuildingTypes });
            state.buildingTypes = dataBuildingTypes;
            //this.setState({ buildingTypes: dataBuildingTypes });
        }

        const dataBuildings = await this.requestService.getBuildings();
        dataBuildings.forEach(element => {
            const buildingType = state.buildingTypes.find(b => b.id == element.buildingTypeId);
            if (buildingType != null) {
                element.name = buildingType.name;
                element.desc = buildingType.description;
            }
        });

        store.dispatch({ type: 'LOAD_COMPLETE', payload: dataBuildings });
        // this.setState({ buildings: dataBuildings, loading: false });
    }

    async addBuilding(x, y, id) {
        await this.requestService.addBuilding(x, y, id);
        this.getUserData();
    }

    async upBuilding(id) {
        await this.requestService.upBuilding(id);
        this.getUserData();
    }

    async downBuilding(id) {
        await this.requestService.downBuilding(id);
        this.getUserData();
    }

    async removeBuilding(id) {
        await this.requestService.removeBuilding(id);
        this.getUserData();
    }
}

const mapStateToProps = (state) => {
    return {
        //user: state.user,
        //buildingTypes: state.buildingTypes,
        buildings: state.game.buildings,
        coins: state.game.coins,
        loading: state.game.loading
    }
}

export default connect(mapStateToProps)(Home);
