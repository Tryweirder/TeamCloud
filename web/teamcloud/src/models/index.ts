/*
 * Copyright (c) Microsoft Corporation.
 * Licensed under the MIT License.
 *
 * Code generated by Microsoft (R) AutoRest Code Generator.
 * Changes may cause incorrect behavior and will be lost if the code is regenerated.
 */

import * as coreHttp from "@azure/core-http";

export interface DeploymentScopeListDataResult {
  code?: number;
  status?: string | null;
  /**
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly data?: DeploymentScope[] | null;
  location?: string | null;
}

export interface DeploymentScope {
  organization: string;
  /**
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly slug: string;
  displayName: string;
  isDefault: boolean;
  managementGroupId?: string | null;
  subscriptionIds?: string[] | null;
  id: string;
}

export interface ErrorResult {
  code?: number;
  status?: string | null;
  errors?: ResultError[] | null;
}

export interface ResultError {
  code?: ResultErrorCode;
  message?: string | null;
  errors?: ValidationError[] | null;
}

export interface ValidationError {
  field?: string | null;
  message?: string | null;
}

export interface DeploymentScopeDefinition {
  /**
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly slug?: string | null;
  displayName: string;
  isDefault?: boolean;
  managementGroupId?: string | null;
  subscriptionIds?: string[] | null;
}

export interface DeploymentScopeDataResult {
  code?: number;
  status?: string | null;
  data?: DeploymentScope;
  location?: string | null;
}

export interface OrganizationListDataResult {
  code?: number;
  status?: string | null;
  /**
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly data?: Organization[] | null;
  location?: string | null;
}

export interface Organization {
  tenant: string;
  /**
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly slug: string;
  displayName: string;
  /**
   * Dictionary of <string>
   */
  tags?: { [propertyName: string]: string } | null;
  id: string;
}

export interface OrganizationDefinition {
  /**
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly slug?: string | null;
  displayName: string;
}

export interface OrganizationDataResult {
  code?: number;
  status?: string | null;
  data?: Organization;
  location?: string | null;
}

export interface StatusResult {
  code?: number;
  status?: string | null;
  /**
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly state?: string | null;
  stateMessage?: string | null;
  location?: string | null;
  errors?: ResultError[] | null;
  trackingId?: string | null;
}

export interface UserListDataResult {
  code?: number;
  status?: string | null;
  /**
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly data?: User[] | null;
  location?: string | null;
}

export interface User {
  organization: string;
  userType: UserType;
  role: UserRole;
  projectMemberships?: ProjectMembership[] | null;
  /**
   * Dictionary of <string>
   */
  properties?: { [propertyName: string]: string } | null;
  id: string;
}

export interface ProjectMembership {
  projectId: string;
  role: ProjectMembershipRole;
  /**
   * Dictionary of <string>
   */
  properties?: { [propertyName: string]: string } | null;
}

export interface UserDefinition {
  identifier: string;
  role: string;
  /**
   * Dictionary of <string>
   */
  properties?: { [propertyName: string]: string } | null;
}

export interface UserDataResult {
  code?: number;
  status?: string | null;
  data?: User;
  location?: string | null;
}

export interface ProjectListDataResult {
  code?: number;
  status?: string | null;
  /**
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly data?: Project[] | null;
  location?: string | null;
}

export interface Project {
  organization: string;
  /**
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly slug: string;
  displayName: string;
  template: string;
  templateInput?: string | null;
  users?: User[] | null;
  /**
   * Dictionary of <string>
   */
  tags?: { [propertyName: string]: string } | null;
  id: string;
}

export interface ProjectDefinition {
  /**
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly slug?: string | null;
  displayName: string;
  template: string;
  templateInput: string;
  users?: UserDefinition[] | null;
}

export interface ProjectDataResult {
  code?: number;
  status?: string | null;
  data?: Project;
  location?: string | null;
}

export interface ComponentListDataResult {
  code?: number;
  status?: string | null;
  /**
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly data?: Component[] | null;
  location?: string | null;
}

export interface Component {
  href?: string | null;
  organization: string;
  templateId: string;
  projectId: string;
  provider: string;
  requestedBy: string;
  displayName?: string | null;
  description?: string | null;
  inputJson?: string | null;
  valueJson?: string | null;
  type: ComponentType;
  resourceId?: string | null;
  id: string;
}

export interface ComponentRequest {
  templateId: string;
  inputJson?: string | null;
}

export interface ComponentDataResult {
  code?: number;
  status?: string | null;
  data?: Component;
  location?: string | null;
}

export interface ComponentTemplateListDataResult {
  code?: number;
  status?: string | null;
  /**
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly data?: ComponentTemplate[] | null;
  location?: string | null;
}

export interface ComponentTemplate {
  organization: string;
  parentId: string;
  provider?: string | null;
  displayName?: string | null;
  description?: string | null;
  repository: RepositoryReference;
  inputJsonSchema?: string | null;
  type: ComponentTemplateType;
  id: string;
}

export interface RepositoryReference {
  url: string;
  token?: string | null;
  version?: string | null;
  baselUrl?: string | null;
  mountUrl?: string | null;
  ref?: string | null;
  provider: RepositoryReferenceProvider;
  type: RepositoryReferenceType;
  organization?: string | null;
  repository?: string | null;
  project?: string | null;
}

export interface ComponentTemplateDataResult {
  code?: number;
  status?: string | null;
  data?: ComponentTemplate;
  location?: string | null;
}

export interface StringDictionaryDataResult {
  code?: number;
  status?: string | null;
  /**
   * Dictionary of <string>
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly data?: { [propertyName: string]: string } | null;
  location?: string | null;
}

export interface ProjectTemplateListDataResult {
  code?: number;
  status?: string | null;
  /**
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly data?: ProjectTemplate[] | null;
  location?: string | null;
}

export interface ProjectTemplate {
  organization: string;
  /**
   * NOTE: This property will not be serialized. It can only be populated by the server.
   */
  readonly slug: string;
  name?: string | null;
  displayName: string;
  components?: string[] | null;
  repository: RepositoryReference;
  description?: string | null;
  isDefault: boolean;
  inputJsonSchema?: string | null;
  id: string;
}

export interface ProjectTemplateDefinition {
  displayName: string;
  repository: RepositoryDefinition;
}

export interface RepositoryDefinition {
  url: string;
  token?: string | null;
  version?: string | null;
}

export interface ProjectTemplateDataResult {
  code?: number;
  status?: string | null;
  data?: ProjectTemplate;
  location?: string | null;
}

/**
 * Defines values for ResultErrorCode.
 */
export type ResultErrorCode =
  | "Unknown"
  | "Failed"
  | "Conflict"
  | "NotFound"
  | "ServerError"
  | "ValidationError"
  | "Unauthorized"
  | "Forbidden"
  | string;
/**
 * Defines values for UserType.
 */
export type UserType = "User" | "System" | "Provider" | "Application" | string;
/**
 * Defines values for UserRole.
 */
export type UserRole = "None" | "Provider" | "Creator" | "Admin" | string;
/**
 * Defines values for ProjectMembershipRole.
 */
export type ProjectMembershipRole =
  | "None"
  | "Provider"
  | "Member"
  | "Owner"
  | string;
/**
 * Defines values for ComponentType.
 */
export type ComponentType =
  | "Custom"
  | "AzureResource"
  | "Environment"
  | "GitRepository"
  | string;
/**
 * Defines values for RepositoryReferenceProvider.
 */
export type RepositoryReferenceProvider =
  | "Unknown"
  | "GitHub"
  | "DevOps"
  | string;
/**
 * Defines values for RepositoryReferenceType.
 */
export type RepositoryReferenceType =
  | "Unknown"
  | "Tag"
  | "Branch"
  | "Hash"
  | string;
/**
 * Defines values for ComponentTemplateType.
 */
export type ComponentTemplateType =
  | "Custom"
  | "AzureResource"
  | "Environment"
  | "GitRepository"
  | string;

/**
 * Contains response data for the getDeploymentScopes operation.
 */
export type TeamCloudGetDeploymentScopesResponse = DeploymentScopeListDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: DeploymentScopeListDataResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudCreateDeploymentScopeOptionalParams
  extends coreHttp.OperationOptions {
  body?: DeploymentScopeDefinition;
}

/**
 * Contains response data for the createDeploymentScope operation.
 */
export type TeamCloudCreateDeploymentScopeResponse = DeploymentScopeDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: DeploymentScopeDataResult;
  };
};

/**
 * Contains response data for the getDeploymentScope operation.
 */
export type TeamCloudGetDeploymentScopeResponse = DeploymentScopeDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: DeploymentScopeDataResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudUpdateDeploymentScopeOptionalParams
  extends coreHttp.OperationOptions {
  body?: DeploymentScope;
}

/**
 * Contains response data for the updateDeploymentScope operation.
 */
export type TeamCloudUpdateDeploymentScopeResponse = DeploymentScopeDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: DeploymentScopeDataResult;
  };
};

/**
 * Contains response data for the deleteDeploymentScope operation.
 */
export type TeamCloudDeleteDeploymentScopeResponse = DeploymentScopeDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: DeploymentScopeDataResult;
  };
};

/**
 * Contains response data for the getOrganizations operation.
 */
export type TeamCloudGetOrganizationsResponse = OrganizationListDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: OrganizationListDataResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudCreateOrganizationOptionalParams
  extends coreHttp.OperationOptions {
  body?: OrganizationDefinition;
}

/**
 * Contains response data for the createOrganization operation.
 */
export type TeamCloudCreateOrganizationResponse = OrganizationDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: OrganizationDataResult;
  };
};

/**
 * Contains response data for the getOrganization operation.
 */
export type TeamCloudGetOrganizationResponse = OrganizationDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: OrganizationDataResult;
  };
};

/**
 * Contains response data for the deleteOrganization operation.
 */
export type TeamCloudDeleteOrganizationResponse = StatusResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: StatusResult;
  };
};

/**
 * Contains response data for the getOrganizationUsers operation.
 */
export type TeamCloudGetOrganizationUsersResponse = UserListDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: UserListDataResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudCreateOrganizationUserOptionalParams
  extends coreHttp.OperationOptions {
  body?: UserDefinition;
}

/**
 * Contains response data for the createOrganizationUser operation.
 */
export type TeamCloudCreateOrganizationUserResponse = UserDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: UserDataResult;
  };
};

/**
 * Contains response data for the getOrganizationUser operation.
 */
export type TeamCloudGetOrganizationUserResponse = UserDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: UserDataResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudUpdateOrganizationUserOptionalParams
  extends coreHttp.OperationOptions {
  body?: User;
}

/**
 * Contains response data for the updateOrganizationUser operation.
 */
export type TeamCloudUpdateOrganizationUserResponse = StatusResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: StatusResult;
  };
};

/**
 * Contains response data for the deleteOrganizationUser operation.
 */
export type TeamCloudDeleteOrganizationUserResponse = StatusResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: StatusResult;
  };
};

/**
 * Contains response data for the getOrganizationUserMe operation.
 */
export type TeamCloudGetOrganizationUserMeResponse = UserDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: UserDataResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudUpdateOrganizationUserMeOptionalParams
  extends coreHttp.OperationOptions {
  body?: User;
}

/**
 * Contains response data for the updateOrganizationUserMe operation.
 */
export type TeamCloudUpdateOrganizationUserMeResponse = StatusResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: StatusResult;
  };
};

/**
 * Contains response data for the getProjects operation.
 */
export type TeamCloudGetProjectsResponse = ProjectListDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: ProjectListDataResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudCreateProjectOptionalParams
  extends coreHttp.OperationOptions {
  body?: ProjectDefinition;
}

/**
 * Contains response data for the createProject operation.
 */
export type TeamCloudCreateProjectResponse = ProjectDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: ProjectDataResult;
  };
};

/**
 * Contains response data for the getProject operation.
 */
export type TeamCloudGetProjectResponse = ProjectDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: ProjectDataResult;
  };
};

/**
 * Contains response data for the deleteProject operation.
 */
export type TeamCloudDeleteProjectResponse = StatusResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: StatusResult;
  };
};

/**
 * Contains response data for the getProjectComponents operation.
 */
export type TeamCloudGetProjectComponentsResponse = ComponentListDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: ComponentListDataResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudCreateProjectComponentOptionalParams
  extends coreHttp.OperationOptions {
  body?: ComponentRequest;
}

/**
 * Contains response data for the createProjectComponent operation.
 */
export type TeamCloudCreateProjectComponentResponse = ComponentDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: ComponentDataResult;
  };
};

/**
 * Contains response data for the getProjectComponent operation.
 */
export type TeamCloudGetProjectComponentResponse = ComponentDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: ComponentDataResult;
  };
};

/**
 * Contains response data for the deleteProjectComponent operation.
 */
export type TeamCloudDeleteProjectComponentResponse = StatusResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: StatusResult;
  };
};

/**
 * Contains response data for the getProjectComponentTemplates operation.
 */
export type TeamCloudGetProjectComponentTemplatesResponse = ComponentTemplateListDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: ComponentTemplateListDataResult;
  };
};

/**
 * Contains response data for the getProjectComponentTemplate operation.
 */
export type TeamCloudGetProjectComponentTemplateResponse = ComponentTemplateDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: ComponentTemplateDataResult;
  };
};

/**
 * Contains response data for the getProjectTags operation.
 */
export type TeamCloudGetProjectTagsResponse = StringDictionaryDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: StringDictionaryDataResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudCreateProjectTagOptionalParams
  extends coreHttp.OperationOptions {
  /**
   * Dictionary of <string>
   */
  body?: { [propertyName: string]: string };
}

/**
 * Contains response data for the createProjectTag operation.
 */
export type TeamCloudCreateProjectTagResponse = StatusResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: StatusResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudUpdateProjectTagOptionalParams
  extends coreHttp.OperationOptions {
  /**
   * Dictionary of <string>
   */
  body?: { [propertyName: string]: string };
}

/**
 * Contains response data for the updateProjectTag operation.
 */
export type TeamCloudUpdateProjectTagResponse = StatusResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: StatusResult;
  };
};

/**
 * Contains response data for the getProjectTagByKey operation.
 */
export type TeamCloudGetProjectTagByKeyResponse = StringDictionaryDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: StringDictionaryDataResult;
  };
};

/**
 * Contains response data for the deleteProjectTag operation.
 */
export type TeamCloudDeleteProjectTagResponse = StatusResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: StatusResult;
  };
};

/**
 * Contains response data for the getProjectTemplates operation.
 */
export type TeamCloudGetProjectTemplatesResponse = ProjectTemplateListDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: ProjectTemplateListDataResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudCreateProjectTemplateOptionalParams
  extends coreHttp.OperationOptions {
  body?: ProjectTemplateDefinition;
}

/**
 * Contains response data for the createProjectTemplate operation.
 */
export type TeamCloudCreateProjectTemplateResponse = ProjectTemplateDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: ProjectTemplateDataResult;
  };
};

/**
 * Contains response data for the getProjectTemplate operation.
 */
export type TeamCloudGetProjectTemplateResponse = ProjectTemplateDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: ProjectTemplateDataResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudUpdateProjectTemplateOptionalParams
  extends coreHttp.OperationOptions {
  body?: ProjectTemplate;
}

/**
 * Contains response data for the updateProjectTemplate operation.
 */
export type TeamCloudUpdateProjectTemplateResponse = ProjectTemplateDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: ProjectTemplateDataResult;
  };
};

/**
 * Contains response data for the deleteProjectTemplate operation.
 */
export type TeamCloudDeleteProjectTemplateResponse = ProjectTemplateDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: ProjectTemplateDataResult;
  };
};

/**
 * Contains response data for the getProjectUsers operation.
 */
export type TeamCloudGetProjectUsersResponse = UserListDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: UserListDataResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudCreateProjectUserOptionalParams
  extends coreHttp.OperationOptions {
  body?: UserDefinition;
}

/**
 * Contains response data for the createProjectUser operation.
 */
export type TeamCloudCreateProjectUserResponse = UserDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: UserDataResult;
  };
};

/**
 * Contains response data for the getProjectUser operation.
 */
export type TeamCloudGetProjectUserResponse = UserDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: UserDataResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudUpdateProjectUserOptionalParams
  extends coreHttp.OperationOptions {
  body?: User;
}

/**
 * Contains response data for the updateProjectUser operation.
 */
export type TeamCloudUpdateProjectUserResponse = UserDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: UserDataResult;
  };
};

/**
 * Contains response data for the deleteProjectUser operation.
 */
export type TeamCloudDeleteProjectUserResponse = StatusResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: StatusResult;
  };
};

/**
 * Contains response data for the getProjectUserMe operation.
 */
export type TeamCloudGetProjectUserMeResponse = UserDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: UserDataResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudUpdateProjectUserMeOptionalParams
  extends coreHttp.OperationOptions {
  body?: User;
}

/**
 * Contains response data for the updateProjectUserMe operation.
 */
export type TeamCloudUpdateProjectUserMeResponse = UserDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: UserDataResult;
  };
};

/**
 * Contains response data for the getStatus operation.
 */
export type TeamCloudGetStatusResponse = StatusResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: StatusResult;
  };
};

/**
 * Contains response data for the getProjectStatus operation.
 */
export type TeamCloudGetProjectStatusResponse = StatusResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: StatusResult;
  };
};

/**
 * Contains response data for the getUserProjects operation.
 */
export type TeamCloudGetUserProjectsResponse = ProjectListDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: ProjectListDataResult;
  };
};

/**
 * Contains response data for the getUserProjectsMe operation.
 */
export type TeamCloudGetUserProjectsMeResponse = ProjectListDataResult & {
  /**
   * The underlying HTTP response.
   */
  _response: coreHttp.HttpResponse & {
    /**
     * The response body as text (string format)
     */
    bodyAsText: string;

    /**
     * The response body as parsed JSON or XML
     */
    parsedBody: ProjectListDataResult;
  };
};

/**
 * Optional parameters.
 */
export interface TeamCloudOptionalParams extends coreHttp.ServiceClientOptions {
  /**
   * Overrides client endpoint.
   */
  endpoint?: string;
}