// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import { AuthenticatedTemplate } from '@azure/msal-react';
import { getTheme, Stack } from '@fluentui/react';
import React from 'react';
import { Redirect, Route, Switch } from 'react-router-dom';
import { ContentRouter, NavRouter } from '.';
import { HeaderBar } from '../components';
import { StateRouter } from '../StateRouter';


export interface IRootViewProps { }

export const RootView: React.FC<IRootViewProps> = (props) => {

    const theme = getTheme();

    const leftStackStyles = {
        root: {
            width: '260px',
            paddingTop: '20px',
            paddingBottom: '10px',
            borderRight: `${theme.palette.neutralLight} solid 1px`
        }
    };

    const rightStackStyles = {
        root: {
            backgroundColor: theme.palette.neutralLighterAlt
        }
    };

    return (
        <Switch>
            <Redirect exact from='/orgs' to='/' />
            <Redirect exact from='/orgs/:orgId/projects' to='/orgs/:orgId' />
            <Redirect exact from='/orgs/:orgId/settings/overview' to='/orgs/:orgId/settings' />
            <Redirect exact from='/orgs/:orgId/projects/:projectId/overview' to='/orgs/:orgId/projects/:projectId' />
            <Redirect exact from='/orgs/:orgId/projects/:projectId/settings/overview' to='/orgs/:orgId/projects/:projectId/settings' />
            <Route exact path={[
                '/',
                '/orgs/new',
                '/orgs/:orgId',
                '/orgs/:orgId/settings',
                '/orgs/:orgId/settings/:settingId',
                '/orgs/:orgId/settings/:settingId/new',
                '/orgs/:orgId/projects/new',
                '/orgs/:orgId/projects/:projectId',
                '/orgs/:orgId/projects/:projectId/settings',
                '/orgs/:orgId/projects/:projectId/settings/:settingId',
                '/orgs/:orgId/projects/:projectId/:navId',
                '/orgs/:orgId/projects/:projectId/:navId/new',
                '/orgs/:orgId/projects/:projectId/:navId/:itemId',
            ]}>
                <StateRouter {...{}}>
                    <Stack verticalFill>
                        <HeaderBar />
                        <AuthenticatedTemplate>
                            <Stack horizontal disableShrink verticalFill verticalAlign='stretch'>
                                <Stack.Item styles={leftStackStyles}>
                                    <NavRouter />
                                </Stack.Item>
                                <Stack.Item grow styles={rightStackStyles}>
                                    <ContentRouter />
                                </Stack.Item>
                            </Stack>
                        </AuthenticatedTemplate>
                    </Stack>
                </StateRouter>
            </Route>
        </Switch>
    );
}
