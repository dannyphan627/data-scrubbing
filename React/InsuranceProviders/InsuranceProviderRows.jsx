import React from "react";
import PropTypes from "prop-types";

const InsuranceProviderRows = props => {
  const onEditClick = () => {
    props.onEdit(props.provider);
  };

  const onDeleteClick = () => {
    props.onDelete(props.provider);
  };

  const renderButtons = () => {
    return props.currentUserRole.includes("SysAdmin") ? (
      <React.Fragment>
        <td>
          <span>
            <button
              type="button"
              round="true"
              icon="true"
              className="btn btn-inverse btn-sm"
              onClick={onEditClick}
            >
              <i className="fa fa-edit"></i>
            </button>
          </span>
        </td>
        <td>
          <span>
            <button
              type="button"
              round="true"
              icon="true"
              className="btn btn-inverse btn-sm"
              onClick={onDeleteClick}
            >
              <i className="fas fa-trash"></i>
            </button>
          </span>
        </td>
      </React.Fragment>
    ) : (
      ""
    );
  };

  return (
    <tr>
      <td>
        <span>{props.provider.name}</span>
      </td>
      <td>
        <span>
          {" "}
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
          ) : (
            ""
          )}
        </span>
      </td>
      {renderButtons()}
    </tr>
  );
};

InsuranceProviderRows.propTypes = {
  onDelete: PropTypes.func,
  onEdit: PropTypes.func,
  provider: PropTypes.shape({
    name: PropTypes.string.isRequired,
    id: PropTypes.number.isRequired,
    siteUrl: PropTypes.string
  }),
  currentUserRole: PropTypes.arrayOf(PropTypes.string).isRequired
};

export default InsuranceProviderRows;
