import {environment} from "../../environments/environment";

export class HttpRoutes {
  private static backendUrl = environment.backendUrl;
  private static personController = 'api/persons';
  private static projectController = 'api/projects';
  private static participationController='api/participation';

  public static personApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.personController}`;
  public static projectApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.projectController}`;
  public static participationApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.participationController}`;
}
