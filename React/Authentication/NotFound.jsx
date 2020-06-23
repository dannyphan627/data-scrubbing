import React from "react";
import PropTypes from "prop-types";
//import logger from "sabio-debug";
import { Link } from "react-router-dom";
import "./notFound.css";

const NotFound = ({ currentUser, history }) => {
  const handleAuthRedir = () => {
    if (currentUser) {
      if (currentUser.roles.includes("SysAdmin")) {
        history.push("/sysAdminDashboard");
      } else if (
        currentUser.roles.includes("Office Manager", "Office Assistant")
      ) {
        history.push("/dashboard");
      } else if (currentUser.roles.includes("Provider")) {
        history.push("/contacts");
      } else if (currentUser.roles.includes("Consumer")) {
        history.push("/");
      }
    } else {
      history.push("/");
    }
  };
  return (
    <div id="root ">
      <div className="authentications">
        <section className="page_404 background">
          <div className=" col-sm-offset-1  text-center">
            <div className="four_zero_four_bg">
              <h1 className="text-center text">404</h1>
            </div>
            <div className="content_box_404">
              <h3 className="bottom_message_text">
                Looks like you are lost...
              </h3>
              <p>This page could not be found</p>
              {currentUser ? (
                <button className="btn link_404" onClick={handleAuthRedir}>
                  <i aria-hidden="true">Back to home</i>
                </button>
              ) : (
                <Link to="/login">
                  <button className="btn btn-primary">
                    <i aria-hidden="true">Take me to login!</i>
                  </button>
                </Link>
              )}
            </div>
          </div>
        </section>
      </div>
    </div>
  );
};

NotFound.propTypes = {
  currentUser: PropTypes.shape({
    roles: PropTypes.arrayOf(PropTypes.string)
  }),
  history: PropTypes.shape({
    push: PropTypes.func
  })
};

export default NotFound;
