﻿import {environment} from "../../environments/environment";

export class HttpRoutes {
  private static backendUrl = environment.backendUrl;
  private static personController = 'api/persons';
  private static projectController = 'api/projects';
  private static participationController = 'api/participations';
  private static taskController = 'api/tasks';
  private static uploadController = 'api/upload';
  private static shiftController = 'api/shift';
  private static availabilityController = 'api/availability';
  private static certificateController = 'api/certificate';
  private static emailController = 'api/email';
  private static requirementController = 'api/requirements';

  public static personApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.personController}`;
  public static projectApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.projectController}`;
  public static participationApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.participationController}`;
  public static taskApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.taskController}`;
  public static uploadApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.uploadController}`;
  public static shiftApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.shiftController}`
  public static availabilityApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.availabilityController}`
  public static certificateApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.certificateController}`
  public static emailApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.emailController}`
  public static requirementApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.requirementController}`
}
