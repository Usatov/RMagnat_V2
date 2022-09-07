import React, { Component } from 'react';
import { store } from './../../store/store';
import Building from '../building/Building'
import { BuildingType } from '../building/BuildingType'

export class Game extends Component {

    //constructor(props) {
    //    super(props);
    //}

    createGameTable = () => {
        const state = store.getState().game;
        const numRows = 7;
        const numCols = 5;

        let table = [];

        for (let i = 0; i < numRows; i++) {
            let children = [];
            
            for (let j = 0; j < numCols; j++) {
                let buildingTypeId = 0;
                let buildingId = 0;
                let buildingDesc = "";
                let buildingInitionalCost = 0;
                let buildingLevel = 0;
                let buildingInitionalCoins = 0;
                const building = this.props.buildings.find(b => b.x === i && b.y === j);
                if (building != null) {
                    buildingId = building.id;
                    buildingTypeId = building.buildingTypeId;
                    buildingDesc = `${building.name} * ${building.level}`;
                    buildingLevel = building.level;
                    const buildingType = state.buildingTypes.find(b => b.id == building.buildingTypeId);
                    if (buildingType != null) {
                        buildingInitionalCost = buildingType.initionalCost;
                        buildingInitionalCoins = buildingType.initionalCoins;
                    }
                }

                children.push(<td key={1000 + i * 10 + j}> <Building key={i * 10 + j}
                    building={buildingTypeId} id={buildingId} x={i} y={j} desc={buildingDesc} owner={this}
                    initionalCost={buildingInitionalCost} initionalCoins={buildingInitionalCoins} level={buildingLevel}
                    /> </td>);
            }
            table.push(<tr key={2000 + i}>{children}</tr>);
        }
        return table;
    }

    createBuildTable = () => {
        const state = store.getState().game;
        let table = [];
        for (let i = 0; i < state.buildingTypes.length; i++) {
            table.push(<tr key={i}><td key={1000 + i}> <BuildingType key={2000 + i} data={state.buildingTypes[i]} /> </td></tr>);
        }
        return table;
    }

    addBuilding = (x, y, id) => {
        this.props.owner.addBuilding(x, y, id);
    }

    upBuilding = (id) => {
        this.props.owner.upBuilding(id);
    }

    downBuilding = (id) => {
        this.props.owner.downBuilding(id);
    }

    removeBuilding = (id) => {
        this.props.owner.removeBuilding(id);
    }

    render() {
        return (
            <table className="gametable" >
                <tbody>
                    {this.createGameTable()}
                </tbody>
            </table>
        );
    }
}