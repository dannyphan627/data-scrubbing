import React, { Component } from "react";
import PropTypes from "prop-types";
import logger from "sabio-debug";
import { getUserId, confirmUser } from "../../services/authService";
import "./register.css";
import { confirmUserGroup } from "../../services/userProfilesService";
import scrubsLogo from "../../assets/images/scrubsLogo.png";

const _logger = logger.extend('ConfirmationPage');

export default class ConfirmationPage extends Component {
  state = {
    isConfirmed: true,
    isActive: 3,
    text: null,
    isLoggedIn: false
  };

  componentDidMount() {
    if (this.props.match.params) {
      getUserId(this.props.match.params.token)
        .then(this.onGetSuccess)
        .catch(this.onGenericFail);
      if (this.props.currentUser) {
        this.setState({ isLoggedIn: true });
      }
    }
  }

	onGetSuccess = (response) => {
		_logger(response);

		if (response.item.tokenType === 3) {
			confirmUserGroup(response.item.id).then(this.onConfirmManagerSuccess).then(this.onGenericFail);
		} else {
			const userConfirm = {
				id: response.item.id,
				isConfirmed: this.state.isConfirmed
			};
			confirmUser(userConfirm).then(this.onConfirmRegisterSuccess).catch(this.onGenericFail);
		}
	};

	onConfirmManagerSuccess = (response) => {
		this.setState((prevState) => {
			return {
				...prevState,
				text: 'Thank you for confirming. You are authorized and now able to edit providers in your group.'
			};
		});
		_logger(response);
	};

	onConfirmRegisterSuccess = (response) => {
		this.setState((prevState) => {
			return {
				...prevState,
				text: 'Your account has been confirmed. Thank you for joining Scrubs Data.'
			};
		});
		_logger(response);
	};

	onGenericFail = (errResponse) => {
		_logger(errResponse);
	};

	handleClick = () => {
		this.props.history.push('/login');
	};

  renderButton = () => {
    return this.state.isLoggedIn ? (
      ""
    ) : (
      <button
        className="btn btn-primary btn-lg col-3"
        onClick={this.handleClick}
      >
        Log In
      </button>
    );
  };

  render() {
    return (
      <div>
        <div className="auth-wrapper d-flex no-block justify-content-center align-items-center">
          <div className="auth-box text-light confirmationBackground">
            <div id="loginform">
              <div className="logo">
                <span className="db">
                  <img width="35%" src={scrubsLogo} alt="logo" />
                </span>
                {/* <h5 className="font-medium m-b-20">Confirmed!</h5> */}
                <div className="container-fluid">
                  <h1 className="font-medium m-b-20 confirmationHeadline">
                    Confirmed!
                  </h1>
                  <p className="lead confirmationHeadline">{this.state.text}</p>
                  {this.renderButton()}
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

    );
  }
}

ConfirmationPage.propTypes = {
  history: PropTypes.shape({
    push: PropTypes.func
  }),
  match: PropTypes.shape({
    params: PropTypes.shape({
      token: PropTypes.string
    })
  }),
  currentUser: PropTypes.shape({
    isLoggedIn: PropTypes.bool
  })
};
