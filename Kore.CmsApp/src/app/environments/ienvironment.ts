export type LogLevel = 'debug' | 'info' | 'warn' | 'error';

export interface IEnvironment {
  production: boolean;
  enableDebugTools: boolean;
  logLevel: LogLevel;
  apiHost: string;
}
