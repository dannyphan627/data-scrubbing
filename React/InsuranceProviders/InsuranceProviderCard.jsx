import React from "react";
import PropTypes from "prop-types";

const InsuranceProviderCards = props => {
  const handleDelete = () => {
    props.onDelete(props.provider);
  };

  const handleEdit = () => {
    props.onEdit(props.provider);
  };

  const addDefaultSrc = e => {
    e.target.src =
      "https://www.aip-benefits.org/assets/img-v2/logos/healthexchange.svg";
  };

  return (
    <div className="col-12 col-md-4">
      <div className="card">
        <div className="container insuranceHeader">
          <div className="card-body topCard">
            <div className="card-title-header">{props.provider.name}</div>
          </div>
          <img
            width="100%"
            height="200px"
            onError={addDefaultSrc}
            src={props.provider.logo}
            alt="logo"
          />
        </div>
        <div className="card-body insuranceBody">
          <div className="container insuranceBodyContainer">
            <div className="row insuranceUrl">
              <p className="card-text">
                {props.provider.siteUrl ? (
                  <a
                    href={props.provider.siteUrl}
                    target="_blank"
                    rel="noopener noreferrer"
                    className="card-link "
                  >
                    {props.provider.siteUrl.length <= 30
                      ? props.provider.siteUrl
                      : props.provider.siteUrl.slice(0, 30) + "..."}
                  </a>
                ) : null}
              </p>
            </div>
          </div>
        </div>
        <div className="card-footer footerBtn">
          <button
            type="button"
            onClick={handleEdit}
            className="btn btn-success col-6 mr-1"
          >
            Edit
          </button>
          <button
            type="button"
            onClick={handleDelete}
            className="btn btn-secondary col-6"
          >
            Delete
          </button>
        </div>
      </div>
    </div>
  );
};

InsuranceProviderCards.propTypes = {
  onDelete: PropTypes.func,
  onEdit: PropTypes.func,
  provider: PropTypes.shape({
    name: PropTypes.string.isRequired,
    id: PropTypes.number.isRequired,
    siteUrl: PropTypes.string,
    logo: PropTypes.string
  })
};

export default InsuranceProviderCards;
