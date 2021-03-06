// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

import React, { useContext } from 'react';
import { Stack } from '@fluentui/react';
import { MembersCard, ComponentsCard } from '../components';
import { ProjectContext } from '../Context';

export const ProjectOverview: React.FC = () => {

    const { members, onAddUsers, onRemoveUsers } = useContext(ProjectContext);

    return (
        <Stack
            wrap
            horizontal
            horizontalAlign='center'
            verticalAlign='start'>
            <Stack.Item styles={{ root: { minWidth: '60%', marginRight: '16px' } }}>
                <ComponentsCard />
            </Stack.Item>
            <Stack.Item grow styles={{ root: { minWidth: '20%', marginRight: '16px' } }}>
                <MembersCard members={members} onAddUsers={onAddUsers} onRemoveUsers={onRemoveUsers} />
            </Stack.Item>
        </Stack>
    );
}
