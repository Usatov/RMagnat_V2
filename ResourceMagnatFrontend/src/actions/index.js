import { bindActionCreators } from 'redux';
// import { push } from 'connected-react-router';

export const mapDispatchToProps = (dispatch) => {
    return {
        actions: {
            // push: bindActionCreators(push, dispatch),
            // errorActions: bindActionCreators(errorActions, dispatch)
        }
    };
}