import { environment } from '../../environments/environment';

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

	static personApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.personController}`;
	static projectApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.projectController}`;
	static participationApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.participationController}`;
	static taskApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.taskController}`;
	static uploadApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.uploadController}`;
	static shiftApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.shiftController}`;
	static availabilityApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.availabilityController}`;
	static certificateApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.certificateController}`;
	static emailApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.emailController}`;
	static requirementApiUrl = `${HttpRoutes.backendUrl}${HttpRoutes.requirementController}`;
}
